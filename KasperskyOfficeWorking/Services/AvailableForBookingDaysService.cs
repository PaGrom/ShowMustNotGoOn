using System;
using System.Collections.Generic;

namespace KasperskyOfficeWorking.Services
{
    public sealed class AvailableForBookingDaysService
    {
        public IEnumerable<DateTime> Get()
        {
            var today = DateTime.Today;

            var maxDay = today.AddDays(7);

            for (var date = today; date < maxDay; date = date.AddDays(1))
            {
                yield return date;
            }
        }
    }
}
