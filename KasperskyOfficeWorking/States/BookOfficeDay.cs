using System;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Model;
using KasperskyOfficeWorking.Services;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace KasperskyOfficeWorking.States
{
    public sealed class BookOfficeDay : StateBase
    {
        private readonly IStateContext _stateContext;
        private readonly OfficeDayService _officeDayService;

        [Input]
        public DateTime Date { get; set; }

        public BookOfficeDay(IStateContext stateContext, OfficeDayService officeDayService)
        {
            _stateContext = stateContext;
            _officeDayService = officeDayService;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            await _officeDayService.AddOfficeDayAsync(new OfficeDay(Guid.NewGuid()) {Date = Date}, cancellationToken);

            var request = new SendMessageRequest(_stateContext.UpdateContext.SessionContext.User.Id, $"Ждем Вас {Date:dd/MM/yyyy} в офисе!");
            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(request, cancellationToken);
        }
    }
}
