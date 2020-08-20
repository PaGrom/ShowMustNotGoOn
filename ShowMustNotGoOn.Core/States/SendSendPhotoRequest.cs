using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace ShowMustNotGoOn.Core.States
{
    public class SendSendPhotoRequest : StateBase
    {
        private readonly IStateContext _stateContext;

        [Input]
        public SendPhotoRequest SendPhotoRequest { get; set; }

        [Input]
        public InlineKeyboardMarkup InlineKeyboardMarkup { get; set; }

        public SendSendPhotoRequest(IStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            SendPhotoRequest.ReplyMarkup = InlineKeyboardMarkup;
            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(new Request(SendPhotoRequest), cancellationToken);
        }
    }
}
