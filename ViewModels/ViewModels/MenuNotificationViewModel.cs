using ATG_Notifier.Model;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ATG_Notifier.ViewModels.ViewModels
{
    public class MenuNotificationViewModel : ObservableObject
    {
        public MenuNotification MenuNotification { get; }

        public event EventHandler Notification_ReadStatusChanged;

        public MenuNotificationViewModel(MenuNotification menuNotification)
        {
            MenuNotification = menuNotification ?? throw new ArgumentNullException(nameof(menuNotification));

            CloseCommand = new RelayCommand(() => RequestClose?.Invoke(this, EventArgs.Empty));
            OpenSourceCommand = new RelayCommand(() => WebService.OpenWebsite(MenuNotification.Url));
            LostFocusCommand = new RelayCommand(() => this.IsRead = true);
        }

        #region Commands

        public ICommand CloseCommand { get; }

        public ICommand OpenSourceCommand { get; }

        public ICommand LostFocusCommand { get; }

        #endregion

        public int Id
        {
            get => MenuNotification.Id;
            set => MenuNotification.Id = value;
        }

        public string Title
        {
            get => MenuNotification.Title;
            set => MenuNotification.Title = value;
        }

        public string Body
        {
            get => MenuNotification.Body;
            set => MenuNotification.Body = value;
        }

        public string Url
        {
            get => MenuNotification.Url;
            set => MenuNotification.Url = value;
        }

        public DateTime ArrivalTime
        {
            get => MenuNotification.ArrivalTime;
            set => MenuNotification.ArrivalTime = value;
        }

        public bool IsRead
        {
            get => MenuNotification.IsRead;
            set
            {
                if (value != MenuNotification.IsRead)
                {
                    MenuNotification.IsRead = value;

                    NotifyPropertyChanged();
                    Notification_ReadStatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool IsDirty
        {
            get => MenuNotification.IsDirty;
            set => MenuNotification.IsDirty = value;
        }

        #region Event Handlers

        public event EventHandler RequestClose;

        #endregion
    }
}
