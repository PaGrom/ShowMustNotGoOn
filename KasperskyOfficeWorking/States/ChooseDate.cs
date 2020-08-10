using System;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Calendar;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class ChooseDate : StateBase
    {
        private readonly IStateContext _stateContext;

        public ChooseDate(IStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public override Task OnEnter(CancellationToken cancellationToken)
        {
            var request = new SendMessageRequest(
                _stateContext.UpdateContext.SessionContext.User.Id,
                "Выберите дату",
                InlineCalendar.CreateCalendar(null, null, (d => d.Equals(DateTime.Today), "💚")));

            return _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
