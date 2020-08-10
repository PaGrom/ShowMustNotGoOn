using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Text;

namespace KasperskyOfficeWorking.Services
{
    public sealed class CalendarConditionBuilder
    {
        private readonly OfficeDayService _officeDayService;
        private readonly AvailableForBookingDaysService _availableForBookingDaysService;

        public CalendarConditionBuilder(OfficeDayService officeDayService, AvailableForBookingDaysService availableForBookingDaysService)
        {
            _officeDayService = officeDayService;
            _availableForBookingDaysService = availableForBookingDaysService;
        }

        public async Task<IEnumerable<(Func<DateTime, bool> condition, string marker)>> BuildAsync(CancellationToken cancellationToken)
        {
            var conditions = new List<(Func<DateTime, bool> condition, string marker)>();

            var officeDays = await _officeDayService.GetOfficeDaysAsync(cancellationToken);

            foreach (var officeDay in officeDays)
            {
                conditions.Add((d => d.Date == officeDay.Date, DayMarkers.OfficeDayMarker));
            }

            foreach (var date in _availableForBookingDaysService.Get())
            {
                conditions.Add((d => d.Date == date.Date, DayMarkers.AvailableToBookDay));
            }

            return conditions;
        }
    }
}
