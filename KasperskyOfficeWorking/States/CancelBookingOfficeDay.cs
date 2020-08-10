using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Services;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace KasperskyOfficeWorking.States
{
    public sealed class CancelBookingOfficeDay : StateBase
    {
        private readonly IStateContext _stateContext;
        private readonly OfficeDayService _officeDayService;

        [Input]
        public DateTime Date { get; set; }

        public CancelBookingOfficeDay(IStateContext stateContext, OfficeDayService officeDayService)
        {
            _stateContext = stateContext;
            _officeDayService = officeDayService;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            var officeDays = await _officeDayService.GetOfficeDaysAsync(cancellationToken);

            var date = officeDays.Single(o => o.Date.Date == Date.Date);

            await _officeDayService.RemoveOfficeDayAsync(date, cancellationToken);

            var request = new SendMessageRequest(_stateContext.UpdateContext.SessionContext.User.Id, $"Приятной работы {Date:dd/MM/yyyy} из дома!", new ReplyKeyboardRemove());
            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
