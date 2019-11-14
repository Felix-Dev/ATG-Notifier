using ATG_Notifier.Desktop.Helpers;

namespace ATG_Notifier.Desktop.Utilities
{
    public static class AppFeedbackManager
    {
        public static void FlashApplicationTaskbarButton()
        {
            CommonHelpers.RunOnUIThread(() => WindowFeedbackManager.FlashTaskbarButton(Program.MainWindow));
        }
    }
}
