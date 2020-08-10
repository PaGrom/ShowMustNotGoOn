using System;
using System.Threading;
using System.Threading.Tasks;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace KasperskyOfficeWorking.States
{
    public sealed class NotCancelBookingOfficeDay : StateBase
    {
        private readonly IStateContext _stateContext;

        [Input]
        public DateTime Date { get; set; }

        public NotCancelBookingOfficeDay(IStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            var request = new SendMessageRequest(_stateContext.UpdateContext.SessionContext.User.Id, $"До встречи {Date:dd/MM/yyyy} в офисе!", new ReplyKeyboardRemove());
            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
