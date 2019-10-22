using ATG_Notifier.Data;
using ATG_Notifier.Model;
using ATG_Notifier.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ATG_Notifier.ViewModels.ViewModels
{
    public class MenuNotificationListViewModel
    {
        private readonly INotificationRepository repository;

        private ObservableCollection<MenuNotificationViewModel> notifications;

        public ObservableCollection<MenuNotificationViewModel> Notifications => notifications;

        public event EventHandler<NotificationCollectionChangedEventArgs> NotificationCollectionChanged;

        public MenuNotificationListViewModel(INotificationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));

            notifications = new ObservableCollection<MenuNotificationViewModel>();
        }

        public void Add(MenuNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            MenuNotificationViewModel notificationVm = new MenuNotificationViewModel(notification);
            notificationVm.RequestClose += (s, e) => Remove(s as MenuNotificationViewModel);

            notifications.Add(notificationVm);

            //Program.MainWindow.Invoke(() =>
            //{
            //    notifications.Add(notificationVm);
            //});

            //notification.IsDirty = true;

            NotificationCollectionChanged?.Invoke(this, new NotificationCollectionChangedEventArgs(NotificationCollectionChangedAction.Add, notification));
        }

        public void Remove(MenuNotificationViewModel notificationVm)
        {
            if (notificationVm == null)
            {
                throw new ArgumentNullException(nameof(notificationVm));
            }

            notifications.Remove(notificationVm);
            repository.Remove(notificationVm.MenuNotification);

            NotificationCollectionChanged?.Invoke(this, new NotificationCollectionChangedEventArgs(NotificationCollectionChangedAction.Remove, notificationVm.MenuNotification));
        }

        private void _Remove(MenuNotificationViewModel notification)
        {
            notifications.Remove(notification);
            repository.Remove(notification.MenuNotification);
        }

        public int LoadNotifications()
        {
            notifications = new ObservableCollection<MenuNotificationViewModel>(repository.Get().Select(n => new MenuNotificationViewModel(n)));
            return notifications.Count;
        }

        public void DeleteAllNotifications()
        {
            for (int i = notifications.Count - 1; i >= 0; i--)
            {
                _Remove(notifications[i]);
            }
        }

        public void SaveNotifications()
        {

            MenuNotificationViewModel[] sNotifications = notifications
                .Where(iNotification => !iNotification.IsRead)
                .ToArray();

            //List<MenuNotification> sNotifs = notifications.Where(u => u.IsDirty).ToList();

            foreach (var notif in sNotifications)
            {
                repository.Add(notif.MenuNotification);
            }
        }
    }
}
