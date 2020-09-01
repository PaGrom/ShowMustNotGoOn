using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ShowMustNotGoOn.Core;
using ShowMustNotGoOn.Core.Model;
using Telegrom.Core;

namespace ShowMustNotGoOn.TvShowsService
{
    public class TvShowsService : ITvShowsService
    {
        const string NotFoundImage = "https://images-na.ssl-images-amazon.com/images/I/312yeogBelL._SX466_.jpg";

        private readonly IMyShowsService _myShowsService;
        private readonly IGlobalAttributesService _globalAttributesService;
        private readonly ILogger<TvShowsService> _logger;

        public TvShowsService(IMyShowsService myShowsService,
            IGlobalAttributesService globalAttributesService,
            ILogger<TvShowsService> logger)
        {
            _myShowsService = myShowsService;
            _globalAttributesService = globalAttributesService;
            _logger = logger;
        }

        public async Task<Guid> AddNewTvShowAsync(TvShowDescription tvShowDescription, CancellationToken cancellationToken)
        {
            var existedTvShowDescription = await _globalAttributesService
                .GetGlobalAttributesAsync<TvShowDescription>()
                .SingleOrDefaultAsync(d => d.MyShowsId == tvShowDescription.MyShowsId, cancellationToken);

            if (existedTvShowDescription != null)
            {
                _logger.LogInformation($"TV Show '{tvShowDescription.Title}' (MyShowsId: {tvShowDescription.MyShowsId}) already exists in db");
                return existedTvShowDescription.Id;
            }

            _logger.LogInformation($"Adding TV Show '{tvShowDescription.Title}' (MyShowsId: {tvShowDescription.MyShowsId}) to db");
            tvShowDescription.Id = Guid.NewGuid();
            await _globalAttributesService.CreateOrUpdateGlobalAttributeAsync(tvShowDescription, cancellationToken);

            return tvShowDescription.Id;
        }

        public Task<IEnumerable<TvShowInfo>> SearchTvShowsAsync(string name, CancellationToken cancellationToken)
        {
            return _myShowsService.SearchTvShowsAsync(name, cancellationToken);
        }

        public async Task<TvShowDescription> GetTvShowDescriptionAsync(int myShowsId, CancellationToken cancellationToken)
        {
            var tvShowDescription = await _myShowsService.GetTvShowAsync(myShowsId, cancellationToken);

            if (string.IsNullOrEmpty(tvShowDescription.Image))
            {
                tvShowDescription.Image = NotFoundImage;
            }

            return tvShowDescription;
        }
    }
}
