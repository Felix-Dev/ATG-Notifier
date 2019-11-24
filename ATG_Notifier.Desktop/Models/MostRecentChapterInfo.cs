using System;
using System.Configuration;

namespace ATG_Notifier.Desktop.Models
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    internal class MostRecentChapterInfo
    {
        public MostRecentChapterInfo() {}

        public string NumberAndTitle { get; set; }

        public int WordCount { get; set; }

        public DateTime? ReleaseTime { get; set; }
    }
}
