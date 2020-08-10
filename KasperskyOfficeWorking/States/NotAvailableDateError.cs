using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class NotAvailableDateError : SendMessage
    {
        public NotAvailableDateError(IStateContext stateContext) : base(stateContext, "К сожалению, офисную работу нельзя забронировать на этот день")
        {
        }
    }
}
