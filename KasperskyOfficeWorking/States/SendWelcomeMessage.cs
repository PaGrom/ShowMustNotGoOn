using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public class SendWelcomeMessage : SendMessage
    {
        public SendWelcomeMessage(IStateContext stateContext) 
            : base(stateContext,
                "������!\n" +
                "���� ��� ������� ���� ������������� ����� ��� ������ � �����.\n" +
                "��� ������ ���� �������� �����������, ��� �� ��������� ����������� ��\n" +
                "������ ����� ����� ������������� �����") { }
    }

    public class CantParseEmail : SendMessage
    {
        public CantParseEmail(IStateContext stateContext)
            : base(stateContext,
                "� ���������, �� �� ������ ��������� ���������� ��� email. ��������� ��� � ��������� ��� �����")
        { }
    }
}
