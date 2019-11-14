using ATG_Notifier.Desktop.Views.ToastNotification;
using ATG_Notifier.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Model
{
    public class Settings : ObservableObject
    {
        private static readonly Lazy<Settings> lazy = new Lazy<Settings>(() => new Settings());        

        public static Settings Instance => lazy.Value;

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

        public bool TurnOnDisplay
        {
            get => Properties.Settings.Default.TurnOnDisplay;
            set
            {
                if (value != Properties.Settings.Default.TurnOnDisplay)
                {
                    Properties.Settings.Default.TurnOnDisplay = value;
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

        #endregion // Properties

        #region Creation

        private Settings() { }

        #endregion // Creation

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
