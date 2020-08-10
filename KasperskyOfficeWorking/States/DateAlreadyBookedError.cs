using KasperskyOfficeWorking.Text;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class DateAlreadyBookedError : SendMessage
    {
        public DateAlreadyBookedError(IStateContext stateContext)
            : base(stateContext,
                "Вы уже забронировали данную дату для работы в офисе. Снять бронь?",
                new ReplyKeyboardMarkup
                {
                    OneTimeKeyboard = true,
                    ResizeKeyboard = true,
                    Keyboard = new []
                    {
                        new []
                        {
                            new KeyboardButton(ButtonStrings.CancelBookingOfficeDayYes),
                            new KeyboardButton(ButtonStrings.CancelBookingOfficeDayNo)
                        }
                    }
                })
        { }
    }

    public sealed class WaitCancelBookAnswer : StateBase
    {

    }
}
