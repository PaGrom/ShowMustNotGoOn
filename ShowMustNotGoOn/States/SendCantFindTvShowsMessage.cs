using ShowMustNotGoOn.StateMachine;

namespace ShowMustNotGoOn.States
{
    internal class SendCantFindTvShowsMessage : SendMessage
    {
        public SendCantFindTvShowsMessage(IStateContext stateContext) : base(stateContext, "� ���������, �������� �� ������ ������� �� �������") { }
    }
}
