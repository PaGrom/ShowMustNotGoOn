using System;
using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Model;
using KasperskyOfficeWorking.Services;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace KasperskyOfficeWorking.States
{
    public sealed class SendNewAuthorizationCode : StateBase
    {
        private readonly EmailService _emailService;

        [Output]
        public Email Email { get; set; }

        public SendNewAuthorizationCode(IStateContext stateContext, EmailService emailService)
        {
            _emailService = emailService;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            Email = await _emailService.GetEmailAsync(cancellationToken);
            Email.AuthorizationCode = new Random().Next(100000, 999999);
            await _emailService.AddOrUpdateEmailAsync(Email, cancellationToken);
        }
    }
}
