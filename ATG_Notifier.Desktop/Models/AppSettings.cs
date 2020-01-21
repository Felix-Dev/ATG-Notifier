using ATG_Notifier.Desktop.Views.ToastNotification;

namespace ATG_Notifier.Desktop.Models
{
    internal class AppSettings
    {
        public bool IsDisabledOnFullscreen { get; set; } = false;

        public bool IsFocusModeEnabled { get; set; } = false;

        public bool IsSoundEnabled { get; set; } = true;

        public bool IsStartMinimizedEnabled { get; set; } = false;

        public bool KeepRunningOnClose { get; set; } = true;

        public DisplayPosition NotificationDisplayPosition { get; set; } = DisplayPosition.TopRight;
    }
}
