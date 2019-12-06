using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Services
{
    public class ChapterUpdateEventArgs : EventArgs
    {
        public ChapterUpdateEventArgs(ChapterProfileViewModel chapterProfile)
        {
            this.ChapterProfile = chapterProfile;
        }

        public ChapterProfileViewModel ChapterProfile { get; set; }
    }
}
