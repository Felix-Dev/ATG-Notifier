using ATG_Notifier.Desktop.Views.ToastNotification;
using ATG_Notifier.ViewModels.Infrastructure;
using System;

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
            get
            {
                var data = Properties.Settings.Default.WindowSetting.Split(';', StringSplitOptions.RemoveEmptyEntries);
                if (data.Length < 4)
                {
                    return null;
                }

                if (!int.TryParse(data[0], out int x) || !int.TryParse(data[1], out int y)
                    || !int.TryParse(data[2], out int width) || !int.TryParse(data[3], out int height))
                {
                    return null;
                }

                return new WindowSetting(x, y, width, height);

            }
            set
            {
                if (value != null)
                {
                    Properties.Settings.Default.WindowSetting = $"{value.X};{value.Y};{value.Width};{value.Height}";
                }
                else
                {
                    Properties.Settings.Default.WindowSetting = "";
                }
            }
        }

        public MostRecentChapterInfo MostRecentChapterInfo
        {
            get
            {
                var data = Properties.Settings.Default.MostRecentChapterInfo.Split(";;", StringSplitOptions.RemoveEmptyEntries);
                if (data.Length < 2)
                {
                    return null;
                }

                var info = new MostRecentChapterInfo()
                {
                    NumberAndTitle = data[0]
                };

                if (int.TryParse(data[1], out int wordCount))
                {
                    info.WordCount = wordCount;
                }

                if (data.Length <= 3 && DateTime.TryParse(data[2], out DateTime releaseTime))
                {
                    info.ReleaseTime = releaseTime;
                }

                return info;

            }
            set
            {
                string sMostRecentChapterInfo;
                if (value != null)
                {
                    sMostRecentChapterInfo = $"{value.NumberAndTitle};;{value.WordCount};;{(value.ReleaseTime.HasValue ? value.ReleaseTime.Value.ToString() : "")}";
                }
                else
                {
                    sMostRecentChapterInfo = "";
                }

                if (sMostRecentChapterInfo != Properties.Settings.Default.MostRecentChapterInfo)
                {
                    Properties.Settings.Default.MostRecentChapterInfo = sMostRecentChapterInfo;
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
