namespace ATG_Notifier.Desktop.Services
{
    internal class ChapterProfilesUnreadCountChangedEventArgs
    {
        public ChapterProfilesUnreadCountChangedEventArgs(int count)
        {
            this.Count = count;
        }

        public int Count { get; }
    }
}