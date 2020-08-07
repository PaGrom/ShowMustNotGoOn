using System;
using System.Net.Mail;
using System.Threading.Tasks;
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

            var waitEmailState = sendWelcomeMessageState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new ElseState(typeof(WaitEmail)));

            var (saveEmailState, cantParseEmailState) = waitEmailState
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message message
                                               && TryParseEmail(message.Text.Trim(), out var mail)
                                               && mail.Host == new MailAddress("23@kaspersky.com").Host),
                        typeof(SaveEmail)),
                    new ElseState(typeof(CantParseEmail)));

            cantParseEmailState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new ElseState(waitEmailState));

            var (emailAlreadyRegisteredErrorState, sendEmailWithCodeState) = saveEmailState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new IfState(
                        ctx => Task.FromResult((bool) ctx.Attributes[nameof(SaveEmail.EmailAlreadyRegistered)].value),
                        typeof(EmailAlreadyRegisteredError)),
                    new ElseState(typeof(SendEmailWithCode)));

            emailAlreadyRegisteredErrorState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new ElseState(waitEmailState));

            var askAuthorizationCodeState = sendEmailWithCodeState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new ElseState(typeof(AskAuthorizationCode)));

            var waitAuthorizationCodeState = askAuthorizationCodeState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new ElseState(typeof(WaitAuthorizationCode)));

            var (authorizationCodeIsWrongErrorState, authorizationCodeIsOkState) = waitAuthorizationCodeState
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(!(bool)ctx.Attributes[nameof(WaitAuthorizationCode.AuthorizationCodeIsOk)].value),
                        typeof(AuthorizationCodeIsWrongError)),
                    new ElseState(typeof(AuthorizationCodeIsOk)));

            authorizationCodeIsWrongErrorState
                .SetNext(
                    NextStateKind.AfterOnEnter,
                    new ElseState(waitAuthorizationCodeState));

            var defaultHandleUpdateState = authorizationCodeIsOkState.SetNext(NextStateKind.AfterOnEnter, new ElseState(typeof(HandleUpdate)));

            var (handleMessageState, handleCallbackQueryState, _) = defaultHandleUpdateState
                .SetNext(
                    NextStateKind.AfterHandle,
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is Message),
                        typeof(HandleMessage)),
                    new IfState(
                        ctx => Task.FromResult(ctx.UpdateContext.Update is CallbackQuery),
                        typeof(HandleCallbackQuery)),
                    new ElseState(defaultHandleUpdateState));

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
