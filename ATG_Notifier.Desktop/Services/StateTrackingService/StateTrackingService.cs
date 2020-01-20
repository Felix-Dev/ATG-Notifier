using ATG_Notifier.Desktop.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Services
{
    internal class StateTrackingService
    {
        private int numUnreadChapters;

        private object updateUnreadChaptersStateLock = new object();

        public StateTrackingService()
        {
            var settingsService = ServiceLocator.Current.GetService<SettingsService>();
            settingsService.SavingAppState += OnSavingAppState;
        }

        public event EventHandler<ChapterProfilesUnreadCountChangedEventArgs>? ChapterProfilesUnreadCountChanged;

        private void OnSavingAppState(object? sender, SavingAppStateEventArgs e)
        {
            e.AppState.UnreadChapters = this.numUnreadChapters;
        }

        public void UpdateUnreadChaptersState(UpdateNumericValueOperation operationKind, int value)
        {
            lock(this.updateUnreadChaptersStateLock)
            {
                switch (operationKind)
                {
                    case UpdateNumericValueOperation.Increase:
                        this.numUnreadChapters += value;
                        break;
                    case UpdateNumericValueOperation.Decrease:
                        this.numUnreadChapters -= value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operationKind));
                }
            }

            this.ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.numUnreadChapters));
        }
    }

    public enum UpdateNumericValueOperation
    {
        Increase = 0,
        Decrease = 1,
    }
}
