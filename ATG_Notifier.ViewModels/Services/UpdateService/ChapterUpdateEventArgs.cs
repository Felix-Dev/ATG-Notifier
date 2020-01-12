using ATG_Notifier.ViewModels.ViewModels;
using System;

namespace ATG_Notifier.ViewModels.Services
{
    public class ChapterUpdateEventArgs : EventArgs
    {
        public ChapterUpdateEventArgs(string sourceChapterId, ChapterProfileViewModel chapterProfile)
        {
            this.SourceChapterId = sourceChapterId;
            this.ChapterProfileViewModel = chapterProfile;
        }

        public string SourceChapterId { get; }

        public ChapterProfileViewModel ChapterProfileViewModel { get; }
    }
}
