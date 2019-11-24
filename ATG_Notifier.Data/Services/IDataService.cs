using ATG_Notifier.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Data.Services
{
    public interface IDataService : IDisposable
    {
        Task<ChapterProfile> GetChapterProfileAsync(long id);

        Task<IList<ChapterProfile>> GetChapterProfilesAsync();

        Task<int> UpdateChapterProfileAsync(ChapterProfile chapterProfile);

        Task<int> UpdateChapterProfilesAsync(params ChapterProfile[] chapterProfiles);

        Task<int> DeleteChapterProfilesAsync(params ChapterProfile[] chapterProfiles);
    }
}
