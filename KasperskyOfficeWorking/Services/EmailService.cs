using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Model;
using Telegrom.Core;

namespace KasperskyOfficeWorking.Services
{
    public sealed class EmailService
    {
        private readonly ISessionAttributesService _sessionAttributesService;

        public EmailService(ISessionAttributesService sessionAttributesService)
        {
            _sessionAttributesService = sessionAttributesService;
        }

        public Task AddOrUpdateEmailAsync(Email email, CancellationToken cancellationToken)
        {
            return _sessionAttributesService.SaveOrUpdateSessionAttributeAsync(email, cancellationToken);
        }

        public async Task<bool> IsEmailAlreadyRegisteredAsync(string mail, CancellationToken cancellationToken)
        {
            var registeredEmails = await _sessionAttributesService
                .FindAttributesInAllSessionsAsync<Email>(e => e.EmailAddress == mail, cancellationToken);

            return registeredEmails.Any();
        }

        public async Task<Email> GetEmailAsync(CancellationToken cancellationToken)
        {
            return (await _sessionAttributesService.GetAllByTypeAsync<Email>(cancellationToken)).SingleOrDefault();
        }
    }
}
