using System;
using Telegrom.Core;

namespace KasperskyOfficeWorking.Model
{
    public sealed class Email : ISessionAttribute
    {
        public Email(Guid guid)
        {
            Id = guid;
        }

        public Guid Id { get; set; }

        public string EmailAddress { get; set; }

        public bool IsAuthorized { get; set; }

        public int AuthorizationCode { get; set; }
    }
}
