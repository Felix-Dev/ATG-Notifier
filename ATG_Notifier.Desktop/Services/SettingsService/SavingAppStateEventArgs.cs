using ATG_Notifier.Desktop.Models;

namespace ATG_Notifier.Desktop.Services
{
    internal class SavingAppStateEventArgs
    {
        public SavingAppStateEventArgs(AppState appState)
        {
            this.AppState = appState;
        }

        public AppState AppState { get; }
    }
}