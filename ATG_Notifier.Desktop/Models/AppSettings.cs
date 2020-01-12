using ATG_Notifier.Desktop.Views.ToastNotification;

namespace ATG_Notifier.Desktop.Models
{
    internal class AppSettings
    {
        public bool IsDisabledOnFullscreen { get; set; }

        public bool IsFocusModeEnabled { get; set; }

        public bool IsSoundEnabled { get; set; }

        public bool KeepRunningOnClose { get; set; }

        public DisplayPosition NotificationDisplayPosition { get; set; }
    }
}
