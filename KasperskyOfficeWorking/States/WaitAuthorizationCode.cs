using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Model;
using KasperskyOfficeWorking.Services;
using Telegrom.Core.TelegramModel;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace KasperskyOfficeWorking.States
{
    public sealed class WaitAuthorizationCode : StateBase
    {
        private readonly IStateContext _stateContext;
        private readonly EmailService _emailService;

        [Input]
        public Email Email { get; set; }

        [Output]
        public bool AuthorizationCodeIsOk { get; set; }

        public WaitAuthorizationCode(IStateContext stateContext, EmailService emailService)
        {
            _stateContext = stateContext;
            _emailService = emailService;
        }

        public override async Task Handle(CancellationToken cancellationToken)
        {
            var message = ((Message) _stateContext.UpdateContext.Update).Text.Trim();
            if (int.TryParse(message, out var code) && code == Email.AuthorizationCode)
            {
                var email = await _emailService.GetEmailAsync(cancellationToken);
                email.IsAuthorized = true;
                await _emailService.AddOrUpdateEmailAsync(email, cancellationToken);
                AuthorizationCodeIsOk = true;
            }
        }
    }
}
