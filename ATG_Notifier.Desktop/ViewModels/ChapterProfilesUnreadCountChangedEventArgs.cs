using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.ViewModels
{
    internal class ChapterProfilesUnreadCountChangedEventArgs : EventArgs
    {
        public ChapterProfilesUnreadCountChangedEventArgs(int unreadCount)
        {
            this.UnreadCount = unreadCount;
        }

        public int UnreadCount { get; }
    }
}
