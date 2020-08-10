using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Calendar;
using KasperskyOfficeWorking.Services;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace KasperskyOfficeWorking.States
{
    public sealed class ProcessDateCalendarCallback : StateBase
    {
        [Output]
        public bool DateAlreadyBooked { get; set; }

        [Output]
        public bool NotAvailableDate { get; set; }

        [Output]
        public DateTime Date { get; set; }

        private readonly IStateContext _stateContext;
        private readonly OfficeDayService _officeDayService;
        private readonly AvailableForBookingDaysService _availableForBookingDaysService;

        public ProcessDateCalendarCallback(IStateContext stateContext, OfficeDayService officeDayService, AvailableForBookingDaysService availableForBookingDaysService)
        {
            _stateContext = stateContext;
            _officeDayService = officeDayService;
            _availableForBookingDaysService = availableForBookingDaysService;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            var callbackQuery = (CallbackQuery)_stateContext.UpdateContext.Update;

            var removeMessageRequest = new DeleteMessageRequest(_stateContext.UpdateContext.SessionContext.User.Id, callbackQuery.MessageId);

            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(removeMessageRequest, cancellationToken);

            var calendarData = InlineCalendar.ParseCallback(callbackQuery);

            Date = calendarData.DateTime.Date;

            var officeDays = await _officeDayService.GetOfficeDaysAsync(cancellationToken);

            if (officeDays.Select(d => d.Date.Date).Contains(Date))
            {
                DateAlreadyBooked = true;
                return;
            }

            var availableDays = _availableForBookingDaysService.Get();

            if (!availableDays.Contains(Date))
            {
                NotAvailableDate = true;
            }
        }
    }
}
