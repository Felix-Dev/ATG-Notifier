using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ATG_Notifier.Desktop.Models
{
    internal class MostRecentChapterInfoEqualityComparer : IEqualityComparer<MostRecentChapterInfo>
    {
        public bool Equals([AllowNull] MostRecentChapterInfo chapterInfo1, [AllowNull] MostRecentChapterInfo chapterInfo2)
        {
            if (chapterInfo1 == null && chapterInfo2 == null)
            {
                return true;
            }

            if (chapterInfo1 == null || chapterInfo2 == null)
            {
                return false;
            }

            if (chapterInfo1.ReleaseTime.HasValue != chapterInfo2.ReleaseTime.HasValue)
            {
                return false;
            }

            return chapterInfo1.NumberAndTitle == chapterInfo2.NumberAndTitle
                && chapterInfo1.WordCount == chapterInfo2.WordCount
                && (chapterInfo1.ReleaseTime is DateTime releaseTime1
                        ? releaseTime1 == chapterInfo2.ReleaseTime.Value
                        : true);
        }

        public int GetHashCode([DisallowNull] MostRecentChapterInfo chapterInfo)
        {
            int hashCode = HashCode.Combine(chapterInfo.NumberAndTitle, chapterInfo.WordCount);
            if (chapterInfo.ReleaseTime is DateTime releaseTime)
            {
                hashCode = HashCode.Combine(hashCode, releaseTime);
            }

            return hashCode;
        }
    }
}
