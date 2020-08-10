using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Model;
using Telegrom.Core;

namespace KasperskyOfficeWorking.Services
{
    public sealed class OfficeDayService
    {
        private readonly ISessionAttributesService _sessionAttributesService;

        public OfficeDayService(ISessionAttributesService sessionAttributesService)
        {
            _sessionAttributesService = sessionAttributesService;
        }

        public Task<IEnumerable<OfficeDay>> GetOfficeDaysAsync(CancellationToken cancellationToken)
        {
            return _sessionAttributesService.GetAllByTypeAsync<OfficeDay>(cancellationToken);
        }

        public Task AddOfficeDayAsync(OfficeDay officeDay, CancellationToken cancellationToken)
        {
            return _sessionAttributesService.SaveOrUpdateSessionAttributeAsync(officeDay, cancellationToken);
        }

        public Task RemoveOfficeDayAsync(OfficeDay officeDay, CancellationToken cancellationToken)
        {
            return _sessionAttributesService.RemoveSessionAttributeAsync(officeDay, cancellationToken);
        }
    }
}
