using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Views.ToastNotification;
using ATG_Notifier.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;

namespace ATG_Notifier.Desktop.ViewModels
{
    internal class SettingsViewModel : ObservableObject
    {
        private readonly AppSettings appSettings;
        private readonly AppState appState;

        private readonly IEqualityComparer<LatestUpdateProfile> latestUpdateProfileEqualityComparer;

        public SettingsViewModel(AppSettings settings, AppState state) 
        {
            this.appSettings = settings;
            this.appState = state;

            this.latestUpdateProfileEqualityComparer = new LatestUpdateProfileEqualityComparer();
        }

        public bool IsDisabledOnFullscreen
        {
            get => this.appSettings.IsDisabledOnFullscreen;
            set
            {
                if (value != this.appSettings.IsDisabledOnFullscreen)
                {
                    this.appSettings.IsDisabledOnFullscreen = value;
                    NotifyPropertyChanged();
                }  
            }
        }

        public bool IsFocusModeEnabled
        {
            get => this.appSettings.IsFocusModeEnabled;
            set
            {
                if (value != this.appSettings.IsFocusModeEnabled)
                {
                    this.appSettings.IsFocusModeEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsSoundEnabled
        {
            get => this.appSettings.IsSoundEnabled;
            set
            {
                if (value != this.appSettings.IsSoundEnabled)
                {
                    this.appSettings.IsSoundEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool KeepRunningOnClose
        {
            get => this.appSettings.KeepRunningOnClose;
            set
            {
                if (value != this.appSettings.KeepRunningOnClose)
                {
                    this.appSettings.KeepRunningOnClose = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DisplayPosition NotificationDisplayPosition
        {
            get => this.appSettings.NotificationDisplayPosition;
            set
            {
                if (value != this.appSettings.NotificationDisplayPosition)
                {
                    this.appSettings.NotificationDisplayPosition = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public LatestUpdateProfile? LatestUpdateProfile
        {
            get => this.appState.LatestUpdateProfile;
            set
            {
                if (!this.latestUpdateProfileEqualityComparer.Equals(value, this.appState.LatestUpdateProfile))
                {
                    this.appState.LatestUpdateProfile = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
