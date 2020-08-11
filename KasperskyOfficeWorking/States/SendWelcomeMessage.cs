using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public class SendWelcomeMessage : SendMessage
    {
        public SendWelcomeMessage(IStateContext stateContext) 
            : base(stateContext,
                "Здравствуйте!\n" +
                "Этот бот поможет Вам забронировать место для работы в офисе.\n" +
                "Для начала, Вам придется подтвердить, что Вы являетесь сотрудником ЛК\n" +
                "Напишите адрес своей корпоративной почты (например 23@kaspersky.com)") { }
    }
}
