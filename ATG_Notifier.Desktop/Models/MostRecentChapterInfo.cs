using System;
using System.Configuration;

namespace ATG_Notifier.Desktop.Models
{
    internal class MostRecentChapterInfo
    {
        public const int InvalidWordCount = -1;

        public MostRecentChapterInfo(string numberAndTitle, int wordCount)
        {
            this.NumberAndTitle = numberAndTitle;
            this.WordCount = wordCount;
        }

        public string NumberAndTitle { get; }

        public int WordCount { get; }

        public DateTime? ReleaseTime { get; set; }
    }
}
