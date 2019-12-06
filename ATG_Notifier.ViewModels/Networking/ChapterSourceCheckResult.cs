using ATG_Notifier.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Networking
{
    public class ChapterSourceCheckResult
    {
        public ChapterSourceCheckResult(string chapterId, ChapterProfileModel chapterProfileModel)
        {
            this.ChapterId = chapterId;
            this.ChapterProfileModel = chapterProfileModel;
        }

        public string ChapterId { get; }

        public ChapterProfileModel ChapterProfileModel { get; }
    }
}
