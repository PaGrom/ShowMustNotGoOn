using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public class SendWelcomeMessage : SendMessage
    {
        public SendWelcomeMessage(IStateContext stateContext) 
            : base(stateContext,
                "Привет!\n" +
                "Этот бот поможет тебе забронировать место для работы в офисе.\n" +
                "Для начала тебе придется подтвердить, что ты являешься сотрудником ЛК\n" +
                "Напиши адрес своей корпоративной почты") { }
    }
}
