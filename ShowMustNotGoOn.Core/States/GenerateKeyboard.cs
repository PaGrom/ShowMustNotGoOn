using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace ShowMustNotGoOn.Core.States
{
    public sealed class GenerateKeyboard : StateBase
    {
        [Output]
        public InlineKeyboardMarkup InlineKeyboardMarkup { get; set; }

        public override Task OnEnter(CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup = InlineKeyboardMarkup.Empty();

            return Task.CompletedTask;
        }
    }
}
