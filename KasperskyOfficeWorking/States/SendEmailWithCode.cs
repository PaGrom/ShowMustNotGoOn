using System.Threading;
using System.Threading.Tasks;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class SendEmailWithCode : StateBase
    {
        private readonly IStateContext _stateContext;

        public SendEmailWithCode(IStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public override Task OnEnter(CancellationToken cancellationToken)
        {
            const string message = "На ваш адрес было отправлено письмо c кодом авторизации";

            var request = new SendMessageRequest(_stateContext.UpdateContext.SessionContext.User.Id, message);
            return _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
