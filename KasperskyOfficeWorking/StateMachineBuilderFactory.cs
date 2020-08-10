using System;
using System.Net.Mail;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Extensions;
using KasperskyOfficeWorking.States;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Builder;

namespace KasperskyOfficeWorking
{
    public static class StateMachineBuilderFactory
    {
        public static StateMachineBuilder Create()
        {
            var stateMachineBuilder = new StateMachineBuilder();

            var initStateNode = stateMachineBuilder.AddInit<Start>();

            var (chooseDate, _) = initStateNode
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message message
                                               && message.IsCommand()
                                               && string.Equals(message.Text, "/start",
                                                   StringComparison.InvariantCultureIgnoreCase)),
                        typeof(ChooseDate)),
                    new DefaultState(initStateNode));

            var waitChooseDate = chooseDate
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new DefaultState(typeof(WaitChooseDate)));

            var (processPrevCalendarCallbackState, processNextCalendarCallbackState, processEmptyCalendarCallbackState, _) = waitChooseDate
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(
                            ctx.UpdateContext.Update is CallbackQuery query
                            && InlineCalendar.ParseCallback(query).Type == CalendarCallbackType.Prev),
                        typeof(ProcessPrevCalendarCallback)),
                    new IfState(
                        ctx => Task.FromResult(
                            ctx.UpdateContext.Update is CallbackQuery query
                            && InlineCalendar.ParseCallback(query).Type == CalendarCallbackType.Next),
                        typeof(ProcessNextCalendarCallback)),
                    new IfState(
                        ctx => Task.FromResult(
                            ctx.UpdateContext.Update is CallbackQuery query
                            && InlineCalendar.ParseCallback(query).Type == CalendarCallbackType.Empty),
                        typeof(ProcessEmptyCalendarCallback)),
                    new DefaultState(waitChooseDate));

            processPrevCalendarCallbackState.SetNext(NextStateKind.AfterOnEnter, new DefaultState(waitChooseDate));
            processNextCalendarCallbackState.SetNext(NextStateKind.AfterOnEnter, new DefaultState(waitChooseDate));
            processEmptyCalendarCallbackState.SetNext(NextStateKind.AfterOnEnter, new DefaultState(waitChooseDate));

            return stateMachineBuilder;

            var (sendWelcomeMessageState, _) = initStateNode
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message message
                                               && message.IsCommand()
                                               && string.Equals(message.Text, "/start",
                                                   StringComparison.InvariantCultureIgnoreCase)),
                        typeof(SendWelcomeMessage)),
                    new DefaultState(initStateNode));

            var waitEmailState = sendWelcomeMessageState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new DefaultState(typeof(WaitEmail)));

            var (saveEmailState, cantParseEmailState) = waitEmailState
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message message
                                               && TryParseEmail(message.Text.Trim(), out var mail)
                                               && mail.Host == new MailAddress("23@kaspersky.com").Host),
                        typeof(SaveEmail)),
                    new DefaultState(typeof(CantParseEmail)));

            cantParseEmailState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new DefaultState(waitEmailState));

            var (emailAlreadyRegisteredErrorState, sendEmailWithCodeState) = saveEmailState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new IfState(
                        ctx => Task.FromResult((bool) ctx.Attributes[nameof(SaveEmail.EmailAlreadyRegistered)].value),
                        typeof(EmailAlreadyRegisteredError)),
                    new DefaultState(typeof(SendEmailWithCode)));

            emailAlreadyRegisteredErrorState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new DefaultState(waitEmailState));

            var askAuthorizationCodeState = sendEmailWithCodeState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new DefaultState(typeof(AskAuthorizationCode)));

            var waitAuthorizationCodeState = askAuthorizationCodeState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new DefaultState(typeof(WaitAuthorizationCode)));

            var (changeEmailState, sendNewAuthorizationCodeState, authorizationCodeIsWrongErrorState, authorizationCodeIsOkState) = waitAuthorizationCodeState
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message message && message.Text == ButtonStrings.ChangeEmail),
                        typeof(ChangeEmail)),
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message message && message.Text == ButtonStrings.ResendAuthorizationCode),
                        typeof(SendNewAuthorizationCode)),
                    new IfState(
                        ctx => Task.FromResult(!(bool)ctx.Attributes[nameof(WaitAuthorizationCode.AuthorizationCodeIsOk)].value),
                        typeof(AuthorizationCodeIsWrongError)),
                    new DefaultState(typeof(AuthorizationCodeIsOk)));

            changeEmailState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new DefaultState(waitEmailState));

            sendNewAuthorizationCodeState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new DefaultState(sendEmailWithCodeState));

            authorizationCodeIsWrongErrorState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new DefaultState(waitAuthorizationCodeState));

            var defaultHandleUpdateState = authorizationCodeIsOkState.SetNext(NextStateKind.AfterOnEnter, new DefaultState(typeof(HandleUpdate)));

            var (handleMessageState, handleCallbackQueryState, _) = defaultHandleUpdateState
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message),
                        typeof(HandleMessage)),
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is CallbackQuery),
                        typeof(HandleCallbackQuery)),
                    new DefaultState(defaultHandleUpdateState));

            stateMachineBuilder.SetDefaultStateNode(defaultHandleUpdateState);

            return stateMachineBuilder;
        }

        private static bool TryParseEmail(string email, out MailAddress mail)
        {
            mail = null;
            try
            {
                mail = new MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
