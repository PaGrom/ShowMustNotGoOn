using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class NotAvailableDateError : SendMessage
    {
        public NotAvailableDateError(IStateContext stateContext) : base(stateContext, "К сожалению, работу в офисе нельзя забронировать на этот день")
        {
        }
    }
}
