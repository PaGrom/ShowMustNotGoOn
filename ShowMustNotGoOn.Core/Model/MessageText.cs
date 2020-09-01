using System;
using Telegrom.Core;

namespace ShowMustNotGoOn.Core.Model
{
    public sealed class MessageText : IGlobalAttribute
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
