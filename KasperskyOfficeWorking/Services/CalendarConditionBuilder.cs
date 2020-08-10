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

        public CalendarConditionBuilder(OfficeDayService officeDayService)
        {
            _officeDayService = officeDayService;
        }

        public async Task<IEnumerable<(Func<DateTime, bool> condition, string marker)>> BuildAsync(CancellationToken cancellationToken)
        {
            var conditions = new List<(Func<DateTime, bool> condition, string marker)>();

            var officeDays = await _officeDayService.GetOfficeDaysAsync(cancellationToken);

            foreach (var officeDay in officeDays)
            {
                conditions.Add((d => d.Date == officeDay.Date, DayMarkers.OfficeDayMarker));
            }

            var today = DateTime.Today;

            conditions.Add((d => d.Date >= today.Date && d.Date < today.AddDays(7).Date, DayMarkers.AvailableToBookDay));

            return conditions;
        }
    }
}
