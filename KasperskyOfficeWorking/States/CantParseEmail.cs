using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public class CantParseEmail : SendMessage
    {
        public CantParseEmail(IStateContext stateContext)
            : base(stateContext,
                "� ���������, �� �� ������ ��������� ���������� ��� email. ��������� ��� � ��������� ��� �����")
        { }
    }
}
