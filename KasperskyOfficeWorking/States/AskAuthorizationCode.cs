using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Model;
using KasperskyOfficeWorking.Text;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace KasperskyOfficeWorking.States
{
    public sealed class AskAuthorizationCode : StateBase
    {
        private readonly IStateContext _stateContext;

        [Input]
        public Email Email { get; set; }

        public AskAuthorizationCode(IStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public override Task OnEnter(CancellationToken cancellationToken)
        {
            var message = $"Скопируйте код из письма сюда, пожалуйста (На данный момент мы не отправляем письмо. Ваш код: {Email.AuthorizationCode})";

            var keyboard = new ReplyKeyboardMarkup(
                new[]
                {
                    new []
                    {
                        new KeyboardButton(ButtonStrings.ResendAuthorizationCode)
                    },
                    new []
                    {
                        new KeyboardButton(ButtonStrings.ChangeEmail)
                    }
                }
            )
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            var request = new SendMessageRequest(
                _stateContext.UpdateContext.SessionContext.User.Id,
                message,
                keyboard);

            return _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
