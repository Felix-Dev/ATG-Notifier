using ATG_Notifier.Data.Entities;
using ATG_Notifier.Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATG_Notifier.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<ChapterProfile?> GetChapterProfileAsync(long id)
        {
            return await this.dataSource.ChapterProfiles.Where(r => r.ChapterProfileId == id).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IList<ChapterProfile>> GetChapterProfilesAsync()
        {
            IQueryable<ChapterProfile> items = this.dataSource.ChapterProfiles;

            return await items.ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateChapterProfileAsync(ChapterProfile chapterProfile)
        {
            if (chapterProfile.ChapterProfileId > 0)
            {
                this.dataSource.Entry(chapterProfile).State = EntityState.Modified;
            }
            else
            {
                chapterProfile.ChapterProfileId = UIdGenerator.Next();
                this.dataSource.Entry(chapterProfile).State = EntityState.Added;
            }

            int res = await this.dataSource.SaveChangesAsync().ConfigureAwait(false);
            return res;
        }

        public async Task<int> UpdateChapterProfilesAsync(params ChapterProfile[] chapterProfiles)
        {
            foreach (var chapterProfile in chapterProfiles)
            {
                if (chapterProfile.ChapterProfileId > 0)
                {
                    this.dataSource.Entry(chapterProfile).State = EntityState.Modified;
                }
                else
                {
                    chapterProfile.ChapterProfileId = UIdGenerator.Next();
                    this.dataSource.Entry(chapterProfile).State = EntityState.Added;
                }
            }

            int res = await this.dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteChapterProfilesAsync(params ChapterProfile[] chapterProfiles)
        {
            this.dataSource.ChapterProfiles.RemoveRange(chapterProfiles);
            return await this.dataSource.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
