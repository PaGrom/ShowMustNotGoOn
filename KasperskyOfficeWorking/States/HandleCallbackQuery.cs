using System.Threading;
using System.Threading.Tasks;
using Telegrom.Core;
using Telegrom.StateMachine;

namespace KasperskyOfficeWorking.States
{
    public sealed class HandleCallbackQuery : StateBase
    {
        private readonly IStateContext _stateContext;
        private readonly ISessionAttributesService _sessionAttributesService;

        public HandleCallbackQuery(IStateContext stateContext,
            ISessionAttributesService sessionAttributesService)
        {
            _stateContext = stateContext;
            _sessionAttributesService = sessionAttributesService;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {

        }
    }
}
