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
        private readonly TelegromBot _telegromBot;
        private TelegromClient _telegromClient;

        public KasperskyOfficeBotService(TelegromBot telegromBot)
        {
            _telegromBot = telegromBot;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _telegromClient = _telegromBot.TelegromClient;

            var users = await _telegromClient.GetUsers().ToListAsync(cancellationToken);
            await _telegromClient.SendMessageAsync(users.First(), "23", cancellationToken);
            //var isTodayInOffice = await _telegromClient.GetUserAttributes<OfficeDay>(users.First())
            //    .Where(e => e.Date == DateTime.Today)
            //    .AnyAsync(cancellationToken);
        }
    }
}
