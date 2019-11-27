using System;
using System.Configuration;

namespace ATG_Notifier.Desktop.Models
{
    internal class MostRecentChapterInfo
    {
        public MostRecentChapterInfo() {}

        public string NumberAndTitle { get; set; }

        public int WordCount { get; set; }

        public DateTime? ReleaseTime { get; set; }
    }
}
