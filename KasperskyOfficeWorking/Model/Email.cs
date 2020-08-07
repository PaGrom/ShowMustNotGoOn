using System;
using Telegrom.Core;

namespace KasperskyOfficeWorking.Model
{
    public sealed class Email : ISessionAttribute
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string EmailAddress { get; set; }

        public bool IsAuthorized { get; set; }

        public int AuthorizationCode { get; set; }
    }
}
