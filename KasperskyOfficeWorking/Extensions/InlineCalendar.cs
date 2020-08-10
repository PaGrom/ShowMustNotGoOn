using System;
using System.Collections.Generic;
using System.Linq;
using Telegrom.Core.TelegramModel;

namespace KasperskyOfficeWorking.Extensions
{
    public static class InlineCalendar
    {
        private const char Delimiter = ';';
        private const string PrevMark = "p";
        private const string NextMark = "n";
        private const string EmptyCallback = " ";
        private const string Format = "dd-MM-yyyy";

        public static InlineKeyboardMarkup CreateCalendar(int? yearNumber = null, int? monthNumber = null, params (Func<DateTime, bool> condition, string marker)[] conditionMarkers)
        {
            var now = DateTime.Now;
            
            var year = yearNumber ?? now.Year;
            var month = monthNumber ?? now.Month;

            var calendarButtons = new List<List<InlineKeyboardButton>>();

            calendarButtons.Add(
                new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton
                    {
                        Text = "<",
                        CallbackData = $"{PrevMark}{Delimiter}{year}{Delimiter}{month}"
                    },
                    new InlineKeyboardButton
                    {
                        Text = $"{GetMonthName(month)} {year}",
                        CallbackData = EmptyCallback
                    },
                    new InlineKeyboardButton
                    {
                        Text = ">",
                        CallbackData = $"{NextMark}{Delimiter}{year}{Delimiter}{month}"
                    }
                });

            var daysOfWeek = new List<InlineKeyboardButton>();

            for (int i = 1; i < 8; i++)
            {
                daysOfWeek.Add(new InlineKeyboardButton
                {
                    Text = GetNameOfDayOfWeek(i),
                    CallbackData = EmptyCallback
                });
            }

            calendarButtons.Add(daysOfWeek);

            var daysOfMonthCount = DateTime.DaysInMonth(year, month);

            calendarButtons.Add(new List<InlineKeyboardButton>());

            for (int i = 1; i < daysOfMonthCount + 1; i++)
            {
                var date = new DateTime(year, month, i);
                var dayOfWeek = (int)date.DayOfWeek;

                if (dayOfWeek == 0)
                {
                    dayOfWeek = 7;
                }

                if (i == 1)
                {
                    var countOfEmptyDays = dayOfWeek - 1;
                    for (int j = 0; j < countOfEmptyDays; j++)
                    {
                        calendarButtons.Last().Add(
                            new InlineKeyboardButton 
                            {
                                Text = " ",
                                CallbackData = EmptyCallback
                            });
                    }
                }

                if (dayOfWeek == 1 && i != 1)
                {
                    calendarButtons.Add(new List<InlineKeyboardButton>());
                }

                var dateString = i.ToString();

                foreach (var (condition, marker) in conditionMarkers)
                {
                    if (condition.Invoke(date))
                    {
                        dateString = marker + dateString;
                        break;
                    }
                }

                calendarButtons.Last().Add(
                    new InlineKeyboardButton
                    {
                        Text = dateString,
                        CallbackData = date.ToString(Format)
                    });
            }

            var lastDayOfWeek = (int) new DateTime(year, month, daysOfMonthCount).DayOfWeek;

            if (lastDayOfWeek == 0)
            {
                lastDayOfWeek = 7;
            }

            var daysToEndOfWeek = 7 - lastDayOfWeek;

            for (int i = 0; i < daysToEndOfWeek; i++)
            {
                calendarButtons.Last().Add(
                    new InlineKeyboardButton
                    {
                        Text = " ",
                        CallbackData = EmptyCallback
                    });
            }

            return new InlineKeyboardMarkup(calendarButtons);
        }

        public static CalendarCallbackData ParseCallback(CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data.Equals(EmptyCallback))
            {
                return CalendarCallbackData.Empty;
            }

            if (callbackQuery.Data.StartsWith(PrevMark))
            {
                var split = callbackQuery.Data.Split(Delimiter);

                return new CalendarCallbackData
                {
                    Type = CalendarCallbackType.Prev,
                    DateTime = new DateTime(int.Parse(split[1]), int.Parse(split[2]), 1).AddMonths(-1)
                };
            }

            if (callbackQuery.Data.StartsWith(NextMark))
            {
                var split = callbackQuery.Data.Split(Delimiter);

                return new CalendarCallbackData
                {
                    Type = CalendarCallbackType.Next,
                    DateTime = new DateTime(int.Parse(split[1]), int.Parse(split[2]), 1).AddMonths(1)
                };
            }

            var date = DateTime.ParseExact(callbackQuery.Data, Format, null);

            return new CalendarCallbackData
            {
                Type = CalendarCallbackType.Date,
                DateTime = date
            };
        }

        private static string GetMonthName(int i)
        {
            var date = new DateTime(1970, i, 1);

            return date.ToString("MMMM");
        }

        private static string GetNameOfDayOfWeek(int numberOfDayOfWeek)
        {
            if (numberOfDayOfWeek < 1 || numberOfDayOfWeek > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfDayOfWeek), "Day of week have to be between 1 and 7");
            }

            var date = new DateTime(1970, 1, numberOfDayOfWeek + 4);

            return date.ToString("ddd");
        }
    }
}
