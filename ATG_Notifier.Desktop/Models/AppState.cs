namespace ATG_Notifier.Desktop.Models
{
    internal class AppState
    {
        public bool WasUpdateServiceRunning { get; set; } = false;

        public string? CurrentChapterId { get; set; } = null;

        public WindowLocation? WindowLocation { get; set; } = null;

        public LatestUpdateProfile? LatestUpdateProfile { get; set; } = null;
    }
}
