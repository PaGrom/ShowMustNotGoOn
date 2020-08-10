using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Calendar;
using KasperskyOfficeWorking.Services;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class ProcessPrevCalendarCallback : StateBase
    {
        private readonly IStateContext _stateContext;
        private readonly CalendarConditionBuilder _calendarConditionBuilder;

        public ProcessPrevCalendarCallback(IStateContext stateContext, CalendarConditionBuilder calendarConditionBuilder)
        {
            _stateContext = stateContext;
            _calendarConditionBuilder = calendarConditionBuilder;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            var callbackQuery = (CallbackQuery) _stateContext.UpdateContext.Update;

            var answerCallbackQueryRequest = new AnswerCallbackQueryRequest(callbackQuery.Id);

            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(answerCallbackQueryRequest, cancellationToken);

            var data = InlineCalendar.ParseCallback(callbackQuery);
            var request = new EditMessageReplyMarkupRequest(_stateContext.UpdateContext.SessionContext.User.Id, callbackQuery.MessageId)
            {
                ReplyMarkup = InlineCalendar.CreateCalendar(data.DateTime.Year, data.DateTime.Month, (await _calendarConditionBuilder.BuildAsync(cancellationToken)).ToArray())
            };

            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
