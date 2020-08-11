using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public class CantParseEmail : SendMessage
    {
        public CantParseEmail(IStateContext stateContext)
            : base(stateContext,
                "К сожалению, мы не смогли правильно распознать Ваш email. Проверьте его и отправьте нам снова")
        { }
    }
}
