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
}
