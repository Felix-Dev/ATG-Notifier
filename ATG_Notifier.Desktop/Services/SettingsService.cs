using ATG_Notifier.Desktop.Models;
using System;

namespace ATG_Notifier.Desktop.Services
{
    internal class SettingsService
    {
        private readonly object appSettingsLock = new object();
        private readonly object appStateLock = new object();

        public AppSettings GetAppSettings()
        {
            lock(this.appSettingsLock)
            {
                return GetSettings();
            }
        }

        public AppState GetAppState()
        {
            lock(this.appStateLock)
            {
                return GetState();
            }
        }

        public void SaveAppSettings(AppSettings userSettings)
        {
            lock(this.appSettingsLock)
            {
                SaveSettings(userSettings);
            }
        }

        public void SaveAppState(AppState state)
        {
            lock (this.appStateLock)
            {
                SaveState(state);
            }
        }

        private AppSettings GetSettings()
        {
            return new AppSettings
            {
                IsDisabledOnFullscreen = Properties.UserPreferences.Default.DisableOnFullscreen,
                IsFocusModeEnabled = Properties.UserPreferences.Default.IsFocusModeEnabled,
                IsSoundEnabled = Properties.UserPreferences.Default.IsSoundEnabled,
                KeepRunningOnClose = Properties.UserPreferences.Default.KeepRunningOnClose,
                NotificationDisplayPosition = Properties.UserPreferences.Default.NotificationDisplayPosition
            };
        }

        private void SaveSettings(AppSettings settings)
        {
            Properties.UserPreferences.Default.DisableOnFullscreen = settings.IsDisabledOnFullscreen;
            Properties.UserPreferences.Default.IsFocusModeEnabled = settings.IsFocusModeEnabled;
            Properties.UserPreferences.Default.IsSoundEnabled = settings.IsSoundEnabled;
            Properties.UserPreferences.Default.KeepRunningOnClose = settings.KeepRunningOnClose;
            Properties.UserPreferences.Default.NotificationDisplayPosition = settings.NotificationDisplayPosition;

            Properties.UserPreferences.Default.Save();
        }

        private AppState GetState()
        {
            var appState = new AppState
            {
                CurrentChapterId = Properties.AppState.Default.CurrentChapterId,
                WasUpdateServiceRunning = Properties.AppState.Default.WasUpdateServiceRunning,
            };

            appState.WindowLocation = new WindowLocation()
            {
                X = Properties.AppStateAppWindowLocation.Default.AppPositionX,
                Y = Properties.AppStateAppWindowLocation.Default.AppPositionY,
                Width = Properties.AppStateAppWindowLocation.Default.AppWidth,
                Height = Properties.AppStateAppWindowLocation.Default.AppHeight
            };

            string numberAndTitle = Properties.AppStateMostRecentChapterInfo.Default.NumberAndTitle;
            int wordCount = Properties.AppStateMostRecentChapterInfo.Default.WordCount;

            if (numberAndTitle == "" && wordCount == 0)
            {
                appState.MostRecentChapterInfo = null;
            }
            else
            {
                appState.MostRecentChapterInfo = new MostRecentChapterInfo(numberAndTitle, wordCount);

                if (DateTime.TryParse(Properties.AppStateMostRecentChapterInfo.Default.ReleaseTime, out DateTime releaseTime))
                {
                    appState.MostRecentChapterInfo.ReleaseTime = releaseTime;
                }
            }

            appState.MostRecentChapterInfo = new MostRecentChapterInfo(Properties.AppStateMostRecentChapterInfo.Default.NumberAndTitle, Properties.AppStateMostRecentChapterInfo.Default.WordCount);

            return appState;
        }

        private void SaveState(AppState state)
        {
            Properties.AppState.Default.CurrentChapterId = state.CurrentChapterId;
            Properties.AppState.Default.WasUpdateServiceRunning = state.WasUpdateServiceRunning;

            // App Window Location
            Properties.AppStateAppWindowLocation.Default.AppPositionX = state.WindowLocation.X;
            Properties.AppStateAppWindowLocation.Default.AppPositionY = state.WindowLocation.Y;
            Properties.AppStateAppWindowLocation.Default.AppWidth = state.WindowLocation.Width;
            Properties.AppStateAppWindowLocation.Default.AppHeight = state.WindowLocation.Height;

            // most recent chapter info
            if (state.MostRecentChapterInfo == null)
            {
                Properties.AppStateMostRecentChapterInfo.Default.NumberAndTitle = "";
                Properties.AppStateMostRecentChapterInfo.Default.WordCount = 0;
                Properties.AppStateMostRecentChapterInfo.Default.ReleaseTime = "";
            }
            else
            {
                Properties.AppStateMostRecentChapterInfo.Default.NumberAndTitle = state.MostRecentChapterInfo.NumberAndTitle;
                Properties.AppStateMostRecentChapterInfo.Default.WordCount = state.MostRecentChapterInfo.WordCount;
                Properties.AppStateMostRecentChapterInfo.Default.ReleaseTime = state.MostRecentChapterInfo.ReleaseTime.HasValue
                    ? state.MostRecentChapterInfo.ReleaseTime.Value.ToString()
                    : "";
            }

            Properties.AppState.Default.Save();
            Properties.AppStateAppWindowLocation.Default.Save();
            Properties.AppStateMostRecentChapterInfo.Default.Save();
        }
    }
}
