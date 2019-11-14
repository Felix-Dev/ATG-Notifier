using System;

namespace ATG_Notifier.Model
{
    public class ChapterProfileModel
    {
        public ChapterProfileModel() { }

        public int Id { get; set; }

        public long ChapterId { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string NumberAndTitleFallbackString { get; set; }

        public int WordCount { get; set; }

        public ChapterSource Source { get; set; }

        public string Url { get; set; }

        public DateTime? ReleaseTime { get; set; }

        public DateTime AppArrivalTime { get; set; }

        public bool IsRead { get; set; }
    }
}
