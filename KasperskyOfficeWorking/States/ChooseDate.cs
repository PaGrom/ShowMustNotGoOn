using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Calendar;
using KasperskyOfficeWorking.Services;
using KasperskyOfficeWorking.Text;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class ChooseDate : StateBase
    {
        private readonly IStateContext _stateContext;
        private readonly CalendarConditionBuilder _calendarConditionBuilder;

        public ChooseDate(IStateContext stateContext, CalendarConditionBuilder calendarConditionBuilder)
        {
            _stateContext = stateContext;
            _calendarConditionBuilder = calendarConditionBuilder;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            //var request = new SendMessageRequest(_stateContext.UpdateContext.SessionContext.User.Id, "Выберите дату", new ReplyKeyboardRemove());

            //await _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
            
            var request = new SendMessageRequest(
                _stateContext.UpdateContext.SessionContext.User.Id,
                "Выберите дату\n" +
                $"{DayMarkers.AvailableToBookDay} - дни, в которые можно выйти в офис\n" +
                $"{DayMarkers.OfficeDayMarker} - дни, забронированные вами для работы в офисе",
                InlineCalendar.CreateCalendar(null, null, (await _calendarConditionBuilder.BuildAsync(cancellationToken)).ToArray()));

            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
