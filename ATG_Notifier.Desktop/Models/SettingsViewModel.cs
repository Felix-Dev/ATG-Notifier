using ATG_Notifier.Desktop.Views.ToastNotification;
using ATG_Notifier.ViewModels.Infrastructure;

namespace ATG_Notifier.Desktop.Models
{
    internal class SettingsViewModel : ObservableObject
    {
        public SettingsViewModel() {}

        #region Properties

        public bool DisableOnFullscreen
        {
            get => Properties.Settings.Default.DisableOnFullscreen;
            set
            {
                if (value != Properties.Settings.Default.DisableOnFullscreen)
                {
                    Properties.Settings.Default.DisableOnFullscreen = value;
                    NotifyPropertyChanged();
                }  
            }
        }

        public bool IsUpdateServiceRunning
        {
            get => Properties.Settings.Default.IsUpdateServiceRunning;
            set
            {
                if (value != Properties.Settings.Default.IsUpdateServiceRunning)
                {
                    Properties.Settings.Default.IsUpdateServiceRunning = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CurrentChapterUrl
        {
            get => Properties.Settings.Default.CurrentChapterUrl;
            set => Properties.Settings.Default.CurrentChapterUrl = value;
        }

        public string CurrentChapterId
        {
            get => Properties.Settings.Default.currentChapterId;
            set => Properties.Settings.Default.currentChapterId = value;
        }

        public bool DoNotDisturb
        {
            get => Properties.Settings.Default.DoNotDisturb;
            set
            {
                if (value != Properties.Settings.Default.DoNotDisturb)
                {
                    Properties.Settings.Default.DoNotDisturb = value;
                    NotifyPropertyChanged();
                }                
            }
        }

        public bool PlayPopupSound
        {
            get => Properties.Settings.Default.PlayPopupSound;
            set
            {
                if (value != Properties.Settings.Default.PlayPopupSound)
                {
                    Properties.Settings.Default.PlayPopupSound = value;
                    NotifyPropertyChanged();
                }               
            }
        }

        public DisplayPosition NotificationDisplayPosition
        {
            get => Properties.Settings.Default.NotificationDisplayPosition;
            set
            {
                if (value != Properties.Settings.Default.NotificationDisplayPosition)
                {
                    Properties.Settings.Default.NotificationDisplayPosition = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public WindowSetting WindowSetting
        {
            get => Properties.Settings.Default.WindowSetting;
            set => Properties.Settings.Default.WindowSetting = value;
        }

        public MostRecentChapterInfo MostRecentChapterInfo
        {
            get => Properties.Settings.Default.MostRecentChapterInfo;
            set
            {
                if (value != Properties.Settings.Default.MostRecentChapterInfo)
                {
                    Properties.Settings.Default.MostRecentChapterInfo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion // Properties

        #region Public Methods

        public void Load()
        {
            Properties.Settings.Default.Reload();
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }

        #endregion // Public Methods
    }
}
