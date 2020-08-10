using System;

namespace KasperskyOfficeWorking.Extensions
{
    public sealed class CalendarCallbackData
    {
        public CalendarCallbackType Type { get; set; }

        public DateTime DateTime = default;

        public static CalendarCallbackData Empty => new CalendarCallbackData {Type = CalendarCallbackType.Empty};
    }
}
