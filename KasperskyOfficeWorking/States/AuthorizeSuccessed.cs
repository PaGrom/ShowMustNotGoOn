using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public class AuthorizeSuccessed : SendMessage
    {
        public AuthorizeSuccessed(IStateContext stateContext) : base(stateContext, "Поздравляю! Авторизация прошла успешно!")
        {
        }
    }
}
