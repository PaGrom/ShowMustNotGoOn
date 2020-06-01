using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ShowMustNotGoOn.Core.TelegramModel;
using ShowMustNotGoOn.DatabaseContext.Model;
using ShowMustNotGoOn.StateMachine;
using ShowMustNotGoOn.StateMachine.Builder;
using ShowMustNotGoOn.States;

namespace ShowMustNotGoOn
{
    public class StateMachineBuilderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var stateMachineBuilder = new StateMachineBuilder(builder);

            var initStateNode = stateMachineBuilder.AddInit<Start>();

            var (sendWelcomeMessageState, _) = initStateNode
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message message
                                               && message.IsCommand()
                                               && string.Equals(message.Text, "/start",
                                                   StringComparison.InvariantCultureIgnoreCase)),
                        typeof(SendWelcomeMessage)),
                    new ElseState(initStateNode));

            var defaultHandleUpdateState = sendWelcomeMessageState
                .SetNext(NextStateKind.AfterOnEnter, new ElseState(typeof(HandleUpdate)));

            var (handleMessageState, _) = defaultHandleUpdateState
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message),
                        typeof(HandleMessage)),
                    new ElseState(defaultHandleUpdateState));

            var (handleCommandState, findTvShowsState) = handleMessageState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new IfState(ctx => Task.FromResult(((Message) ctx.UpdateContext.Update).IsCommand()),
                        typeof(HandleCommand)),
                    new ElseState(typeof(FindTvShows)));

            var (generateTvShowsMessageState, sendCantFindTvShowsMessageState) = findTvShowsState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new IfState(
                        ctx =>
                        {
                            var (_, value) = ctx.Attributes[nameof(FindTvShows.TvShows)];
                            var tvShows = (List<TvShow>) value;
                            return Task.FromResult(tvShows.Any());
                        }, typeof(GenerateTvShowsBotMessage)),
                    new ElseState(typeof(SendCantFindTvShowsMessage)));

            sendCantFindTvShowsMessageState
                .SetNext(NextStateKind.AfterOnEnter, new ElseState(defaultHandleUpdateState));

            generateTvShowsMessageState
                .SetNext(NextStateKind.AfterOnEnter, new ElseState(typeof(GenerateSendPhotoWithFirstTvShowRequest)))
                .SetNext(NextStateKind.AfterOnEnter, new ElseState(typeof(GenerateKeyboard)))
                .SetNext(NextStateKind.AfterOnEnter, new ElseState(typeof(GenerateNavigationButtons)))
                .SetNext(NextStateKind.AfterOnEnter, new ElseState(typeof(GenerateSubscriptionsButtons)))
                .SetNext(NextStateKind.AfterOnEnter, new ElseState(typeof(SendSendPhotoRequest)))
                .SetNext(NextStateKind.AfterOnEnter, new ElseState(defaultHandleUpdateState));

            stateMachineBuilder.SetDefaultStateNode(defaultHandleUpdateState);

            stateMachineBuilder.Build();

            builder.RegisterInstance(new StateMachineConfigurationProvider(stateMachineBuilder.InitStateName, stateMachineBuilder.DefaultStateName))
                .As<IStateMachineConfigurationProvider>();
        }
    }
}