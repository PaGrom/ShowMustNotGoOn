using KasperskyOfficeWorking.Text;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class OpenCalendarButton : SendMessage
    {
        public OpenCalendarButton(IStateContext stateContext)
            : base(stateContext, 
                "Откройте календарь",
                new ReplyKeyboardMarkup
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true,
                    Keyboard = new []
                    {
                        new []
                        {
                            new KeyboardButton(ButtonStrings.OpenCalendar)
                        }
                    }
                })
        {
        }
    }
}
