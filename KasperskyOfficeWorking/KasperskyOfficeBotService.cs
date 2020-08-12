using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Model;
using Microsoft.Extensions.Hosting;
using Telegrom;

namespace KasperskyOfficeWorking
{
    public class KasperskyOfficeBotService : BackgroundService
    {
        private readonly TelegromClient _telegromClient;

        public KasperskyOfficeBotService(TelegromClient telegromClient)
        {
            _telegromClient = telegromClient;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var users = await _telegromClient.GetUsers().ToListAsync(cancellationToken);
            var isTodayInOffice = await _telegromClient.GetUserAttributes<OfficeDay>(users.First())
                .Where(e => e.Date == DateTime.Today)
                .AnyAsync(cancellationToken);
        }
    }
}
