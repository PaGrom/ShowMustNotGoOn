using System.Threading;
using System.Threading.Tasks;
using ShowMustNotGoOn.Core.Model;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrom.StateMachine;
using Telegrom.StateMachine.Attributes;

namespace ShowMustNotGoOn.Core.States
{
    public class SendUpdatePhotoRequest : StateBase
    {
        private readonly IStateContext _stateContext;
        private readonly ITvShowsService _tvShowsService;

        [Input]
        public TvShowInfo CurrentTvShowInfo { get; set; }

        [Input]
        public InlineKeyboardMarkup InlineKeyboardMarkup { get; set; }

        public SendUpdatePhotoRequest(IStateContext stateContext, ITvShowsService tvShowsService)
        {
            _stateContext = stateContext;
            _tvShowsService = tvShowsService;
        }

        public override async Task OnEnter(CancellationToken cancellationToken)
        {
            var userId = _stateContext.UpdateContext.SessionContext.User.Id;

            var callbackQuery = _stateContext.UpdateContext.Update.CallbackQuery;

            var answerCallbackQueryRequest = new AnswerCallbackQueryRequest(callbackQuery.Id);

            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(answerCallbackQueryRequest, cancellationToken);

            var tvShowDescription = await _tvShowsService.GetTvShowDescriptionAsync(CurrentTvShowInfo.MyShowsId, cancellationToken);

            var editMessageMediaRequest = new EditMessageMediaRequest(userId, callbackQuery.Message.MessageId, new InputMediaPhoto(new InputMedia(tvShowDescription.Image)));

            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(editMessageMediaRequest, cancellationToken);

            var editCaptionRequest = new EditMessageCaptionRequest(userId, callbackQuery.Message.MessageId, $"{ tvShowDescription.Title } / { tvShowDescription.TitleOriginal}")
            {
                ReplyMarkup = InlineKeyboardMarkup
            };

            await _stateContext.UpdateContext.SessionContext.PostRequestAsync(editCaptionRequest, cancellationToken);
        }
    }
}
