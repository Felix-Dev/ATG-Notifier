using ATG_Notifier.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.ViewModels.Services
{
    public interface IChapterProfileService
    {
        Task<ChapterProfileModel> GetChapterProfileAsync(long id);

        Task<IList<ChapterProfileModel>> GetChapterProfilesAsync();

        Task<int> UpdateChapterProfileAsync(ChapterProfileModel model);

        Task<int> UpdateChapterProfilesAsync(IList<ChapterProfileModel> models);

        Task<int> DeleteChapterProfileAsync(ChapterProfileModel model);

        Task<int> DeleteChapterProfileRangeAsync(IList<ChapterProfileModel> models);
    }
}
