using System;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Model;
using KasperskyOfficeWorking.Services;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace KasperskyOfficeWorking.States
{
    public sealed class SaveEmail : StateBase
    {
        private readonly IStateContext _stateContext;
        private readonly EmailService _emailService;

        [Output]
        public Email Email { get; set; }

        [Output]
        public bool EmailAlreadyRegistered { get; set; }

        public SaveEmail(IStateContext stateContext, EmailService emailService)
        {
            _stateContext = stateContext;
            _emailService = emailService;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            Email = new Email
            {
                EmailAddress = ((Message)_stateContext.UpdateContext.Update).Text.Trim(),
                IsAuthorized = false,
                AuthorizationCode = new Random().Next(100000, 999999)
            };

            if (await _emailService.IsEmailAlreadyRegisteredAsync(Email.EmailAddress, cancellationToken))
            {
                EmailAlreadyRegistered = true;
                return;
            }

            await _emailService.AddOrUpdateEmailAsync(Email, cancellationToken);
        }
    }
}
