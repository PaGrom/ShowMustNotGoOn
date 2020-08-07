using System.Threading;
using System.Threading.Tasks;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public abstract class SendMessage : StateBase
    {
        protected readonly IStateContext StateContext;
        private readonly string _message;
        private readonly IReplyMarkup _keyboardMarkup;

        protected SendMessage(IStateContext stateContext, string message, IReplyMarkup keyboardMarkup = null)
        {
            StateContext = stateContext;
            _message = message;
            _keyboardMarkup = keyboardMarkup;
        }

        public override Task OnEnter(CancellationToken cancellationToken)
        {
            var request = new SendMessageRequest(StateContext.UpdateContext.SessionContext.User.Id, _message, _keyboardMarkup);
            return StateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
