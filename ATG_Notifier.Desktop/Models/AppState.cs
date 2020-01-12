namespace ATG_Notifier.Desktop.Models
{
    internal class AppState
    {
        public bool WasUpdateServiceRunning { get; set; }

        public string? CurrentChapterId { get; set; }

        public WindowLocation WindowLocation { get; set; } = null!;

        public MostRecentChapterInfo? MostRecentChapterInfo { get; set; }
    }
}
