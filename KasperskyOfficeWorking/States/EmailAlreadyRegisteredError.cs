using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class EmailAlreadyRegisteredError : SendMessage
    {
        public EmailAlreadyRegisteredError(IStateContext stateContext)
            : base(stateContext, "К сожалению, адрес, который Вы указали, уже зарегестрирован. Обратитесь к администратору")
        {
        }
    }
}
