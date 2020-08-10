using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class AuthorizationCodeIsWrongError : SendMessage
    {
        public AuthorizationCodeIsWrongError(IStateContext stateContext) : base(stateContext, "Код авторизации неверный")
        {
        }
    }
}
