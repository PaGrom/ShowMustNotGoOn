using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class EmailAlreadyRegisteredError : SendMessage
    {
        public EmailAlreadyRegisteredError(IStateContext stateContext)
            : base(stateContext, "� ���������, �����, ������� �� �������, ��� ���������������. ���������� � ��������������")
        {
        }
    }
}
