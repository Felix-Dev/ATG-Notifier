using System;

namespace ATG_Notifier.Desktop.Models
{
    internal class LatestUpdateProfile
    {
        public LatestUpdateProfile() { }

        public LatestUpdateProfile(string numberAndTitle, int wordCount)
        {
            this.NumberAndTitle = numberAndTitle;
            this.WordCount = wordCount;
        }

        public string NumberAndTitle { get; set; } = "";

        public int WordCount { get; set; }

        public DateTime? ReleaseTime { get; set; }
    }
}
