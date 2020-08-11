﻿using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class AuthorizationCodeIsOk : SendMessage
    {
        public AuthorizationCodeIsOk(IStateContext stateContext)
            : base(stateContext, "Код авторизации верный. Вы успешно зарегистрированы", new ReplyKeyboardRemove())
        {
        }
    }
}
