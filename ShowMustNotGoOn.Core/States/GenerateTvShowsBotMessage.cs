using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ShowMustNotGoOn.Core.Model;
using Telegrom.Core;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace ShowMustNotGoOn.Core.States
{
    public sealed class GenerateTvShowsBotMessage : StateBase
    {
        private readonly IStateContext _stateContext;
        private readonly IGlobalAttributesService _globalAttributesService;
        private readonly ISessionAttributesService _sessionAttributesService;

        [Input]
        public List<TvShowInfo> TvShowsInfos { get; set; }

        [Output]
        public TvShowInfo CurrentTvShowInfo { get; set; }

        [Output]
        public BotMessage BotMessage { get; set; }

        public GenerateTvShowsBotMessage(IStateContext stateContext,
            IGlobalAttributesService globalAttributesService,
            ISessionAttributesService sessionAttributesService)
        {
            _stateContext = stateContext;
            _globalAttributesService = globalAttributesService;
            _sessionAttributesService = sessionAttributesService;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            var messageTextString = _stateContext.UpdateContext.Update.Message.Text.Trim();

            var messageText = await _globalAttributesService
                .GetGlobalAttributesAsync<MessageText>()
                .SingleOrDefaultAsync(t => t.Text == messageTextString, cancellationToken);

            if (messageText == null)
            {
                messageText = new MessageText
                {
                    Id = Guid.NewGuid(),
                    Text = messageTextString
                };

                await _globalAttributesService.CreateOrUpdateGlobalAttributeAsync(messageText, cancellationToken);
            }

            CurrentTvShowInfo = TvShowsInfos.First();

            BotMessage = new BotMessage
            {
                Id = Guid.NewGuid(),
                BotCommandType = BotCommandType.NotCommand,
                MessageTextId = messageText.Id,
                TvShowInfo = CurrentTvShowInfo
            };

            await _sessionAttributesService.SaveOrUpdateSessionAttributeAsync(BotMessage, cancellationToken);
        }
    }
}
