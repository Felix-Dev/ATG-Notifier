namespace ATG_Notifier.Desktop.Services
{
    internal class ChapterProfilesUnreadCountChangedEventArgs
    {
        private int numUnreadChapters;

        public ChapterProfilesUnreadCountChangedEventArgs(int numUnreadChapters)
        {
            this.numUnreadChapters = numUnreadChapters;
        }
    }
}