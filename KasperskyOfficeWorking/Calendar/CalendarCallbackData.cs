using System;

namespace KasperskyOfficeWorking.Calendar
{
    public sealed class CalendarCallbackData
    {
        public CalendarCallbackType Type { get; set; }

        public DateTime DateTime = default;

        public static CalendarCallbackData Empty => new CalendarCallbackData { Type = CalendarCallbackType.Empty };

        public static CalendarCallbackData Unknown => new CalendarCallbackData { Type = CalendarCallbackType.Unknown };
    }
}
