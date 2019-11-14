using ATG_Notifier.Data.Entities;
using ATG_Notifier.Data.Services;
using ATG_Notifier.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.ViewModels.Services
{
    public class ChapterProfileService : IChapterProfileService
    {
        private readonly IDataServiceFactory dataServiceFactory;
        private readonly ILogService logService;

        public ChapterProfileService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            this.dataServiceFactory = dataServiceFactory ?? throw new ArgumentNullException(nameof(dataServiceFactory));
            this.logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<IList<ChapterProfileModel>> GetChapterProfilesAsync(/*DataRequest<Order> request*/)
        {
            IList<ChapterProfile> chapterProfiles = new List<ChapterProfile>();
            using (var dataService = this.dataServiceFactory.CreateDataService())
            {
                chapterProfiles = await dataService.GetChapterProfilesAsync();
            }

            return chapterProfiles
                .Select(profile => CreateChapterProfileModel(profile))
                .ToList();
        }

        public async Task<int> UpdateChapterProfileAsync(ChapterProfileModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            long id = model.ChapterProfileId;
            using (var dataService = dataServiceFactory.CreateDataService())
            {
                var chapterProfile = id > 0 ? await dataService.GetChapterProfileAsync(model.ChapterProfileId) : new ChapterProfile();
                if (chapterProfile != null)
                {
                    UpdateChapterProfileFromModel(chapterProfile, model);
                    await dataService.UpdateChapterProfileAsync(chapterProfile);

                    UpdateModelFromChapterProfile(model, chapterProfile);
                }

                return 0;
            }
        }

        public async Task<int> DeleteChapterProfileAsync(ChapterProfileModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var chapterProfile = new ChapterProfile { ChapterProfileId = model.ChapterProfileId };
            using (var dataService = dataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteChapterProfilesAsync(chapterProfile);
            }
        }

        public async Task<int> DeleteChapterProfileRangeAsync(IList<ChapterProfileModel> models)
        {
            if (models is null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            using (var dataService = dataServiceFactory.CreateDataService())
            {
                IList<ChapterProfile> chapterProfiles = new List<ChapterProfile>();

                foreach (var model in models)
                {
                    var chapterProfile = await dataService.GetChapterProfileAsync(model.ChapterProfileId);
                    chapterProfiles.Add(chapterProfile);
                }

                return await dataService.DeleteChapterProfilesAsync(chapterProfiles.ToArray());
            }
        }

        private ChapterProfileModel CreateChapterProfileModel(ChapterProfile source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var model = new ChapterProfileModel()
            {
                ChapterProfileId = source.ChapterProfileId,
                Number = source.Number,
                Title = source.Title,
                NumberAndTitleFallbackString = source.NumberAndTitleFallbackString,
                WordCount = source.WordCount,
                Url = source.Url,
                ReleaseTime = source.ReleaseTime,
                AppArrivalTime = source.AppArrivalTime,
                IsRead = source.IsRead,
            };

            if (Enum.TryParse(source.ChapterSource.ToString(), out ChapterSource chapterSource))
            {
                model.Source = chapterSource;
            }

            return model;
        }

        private void UpdateModelFromChapterProfile(ChapterProfileModel target, ChapterProfile source)
        {
            target.ChapterProfileId = source.ChapterProfileId;
            target.Number = source.Number;
            target.Title = source.Title;
            target.NumberAndTitleFallbackString = source.NumberAndTitleFallbackString;
            target.WordCount = source.WordCount;

            if (Enum.TryParse(source.ChapterSource.ToString(), out ChapterSource chapterSource))
            {
                target.Source = chapterSource;
            }

            target.Url = source.Url;
            target.ReleaseTime = source.ReleaseTime;
            target.AppArrivalTime = source.AppArrivalTime;
            target.IsRead = source.IsRead;
        }

        private void UpdateChapterProfileFromModel(ChapterProfile target, ChapterProfileModel source)
        {
            target.ChapterProfileId = source.ChapterProfileId;
            target.Number = source.Number;
            target.Title = source.Title;
            target.NumberAndTitleFallbackString = source.NumberAndTitleFallbackString;
            target.WordCount = source.WordCount;
            target.ChapterSource = (int)source.Source;
            target.Url = source.Url;
            target.ReleaseTime = source.ReleaseTime;
            target.AppArrivalTime = source.AppArrivalTime;
            target.IsRead = source.IsRead;
        }
    }
}
