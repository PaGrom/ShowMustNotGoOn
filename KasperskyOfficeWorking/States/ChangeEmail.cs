using System.Threading;
using System.Threading.Tasks;
using KasperskyOfficeWorking.Services;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class ChangeEmail : SendMessage
    {
        private readonly EmailService _emailService;

        public ChangeEmail(IStateContext stateContext, EmailService emailService) : base(stateContext, "Введите новый email")
        {
            _emailService = emailService;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            await _emailService.RemoveEmailForUserAsync(cancellationToken);
            await base.OnEnter(cancellationToken);
        }
    }
}
