using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Models
{
    public class ChapterProfileModel
    {
        public ChapterProfileModel() { }

        public long ChapterProfileId { get; set; }

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
