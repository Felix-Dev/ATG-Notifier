using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UI.Model
{
    public class Settings : INotifyPropertyChanged
    {
        private static readonly Lazy<Settings> lazy = new Lazy<Settings>(() => new Settings());        

        public static Settings Instance => lazy.Value;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members

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

        public string CurrentChapterNumberAndTitle
        {
            get => Properties.Settings.Default.CurrentChapterNumberAndTitle;
            set => Properties.Settings.Default.CurrentChapterNumberAndTitle = value;
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
