using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class DateAlreadyBookedError : SendMessage
    {
        public DateAlreadyBookedError(IStateContext stateContext) : base(stateContext, "Вы уже забронировали данную дату для работы в офисе")
        {
        }
    }
}
