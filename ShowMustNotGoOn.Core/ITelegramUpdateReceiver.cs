﻿using System;
using System.Threading;
using Telegram.Bot.Types;

namespace ShowMustNotGoOn.Core
{
    public interface ITelegramUpdateReceiver : IDisposable
    {
        void Start();
        void SetUpdateReceivedHandler(Action<Update, CancellationToken> handler);
    }
}
