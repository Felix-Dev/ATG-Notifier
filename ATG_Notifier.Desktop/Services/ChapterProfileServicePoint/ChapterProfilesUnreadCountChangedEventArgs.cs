using System;

namespace ATG_Notifier.Desktop.Services
{
    internal class ChapterProfilesUnreadCountChangedEventArgs : EventArgs
    {
        public ChapterProfilesUnreadCountChangedEventArgs(int count)
        {
            this.Count = count;
        }

        public int Count { get; }
    }
}