using System;
using Telegrom.Core;

namespace KasperskyOfficeWorking.Model
{
    public sealed class OfficeDay : ISessionAttribute
    {
        public OfficeDay(Guid guid)
        {
            Id = guid;
        }

        public Guid Id { get; set; }

        public DateTime Date { get; set; }
    }
}
