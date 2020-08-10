using System;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Extensions;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class ProcessNextCalendarCallback : StateBase
    {
        private readonly IStateContext _stateContext;

        public ProcessNextCalendarCallback(IStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            var callbackQuery = (CallbackQuery)_stateContext.UpdateContext.Update;

            var answerCallbackQueryRequest = new AnswerCallbackQueryRequest(callbackQuery.Id);

            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(answerCallbackQueryRequest, cancellationToken);

            var data = InlineCalendar.ParseCallback(callbackQuery);
            var request = new EditMessageReplyMarkupRequest(_stateContext.UpdateContext.SessionContext.User.Id, callbackQuery.MessageId)
            {
                ReplyMarkup = InlineCalendar.CreateCalendar(data.DateTime.Year, data.DateTime.Month, (d => d.Equals(DateTime.Today), "💚"))
            };

            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
