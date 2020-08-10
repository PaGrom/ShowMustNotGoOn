using System.Threading;
using System.Threading.Tasks;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class ProcessEmptyCalendarCallback : StateBase
    {
        private readonly IStateContext _stateContext;

        public ProcessEmptyCalendarCallback(IStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public override Task OnEnter(CancellationToken cancellationToken)
        {
            var callbackQuery = (CallbackQuery)_stateContext.UpdateContext.Update;

            var answerCallbackQueryRequest = new AnswerCallbackQueryRequest(callbackQuery.Id);

            return _stateContext.UpdateContext.SessionContext.PostRequestAsync(answerCallbackQueryRequest, cancellationToken);
        }
    }
}
