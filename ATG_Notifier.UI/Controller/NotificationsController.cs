using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ATG_Notifier.Utilities.Extensions;
using ATG_Notifier.Data;
using ATG_Notifier.UI;
using ATG_Notifier.Model;
using ATG_Notifier.ViewModels.ViewModels;
using ATG_Notifier.ViewModels.Infrastructure;

namespace ATG_Notifier.Controller
{
    public class NotificationsController
    {
        private ObservableCollection<MenuNotificationViewModel> notifications;

        public IReadOnlyList<MenuNotificationViewModel> Notifications => notifications;

        public event EventHandler<NotificationCollectionChangedEventArgs> NotificationCollectionChanged;

        private INotificationRepository repository;

        #region Creation

        // TODO: Bad code!
        private static readonly Lazy<NotificationsController> lazy = new Lazy<NotificationsController>(() => new NotificationsController(Program.Repository.Notifications));

        public static NotificationsController Instance => lazy.Value;

        private NotificationsController(INotificationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));

            notifications = new ObservableCollection<MenuNotificationViewModel>();
        }

        #endregion // Creation

        #region Public Methods

        public void Add(MenuNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            MenuNotificationViewModel notificationVm = new MenuNotificationViewModel(notification);
            notificationVm.RequestClose += (s, e) => Remove(s as MenuNotificationViewModel);
            notificationVm.Notification_ReadStatusChanged += Notification_OnReadStatusChanged;

            Program.MainWindow.Invoke(() =>
            {
                notifications.Add(notificationVm);
            });

            //notification.IsDirty = true;

            NotificationCollectionChanged?.Invoke(this, new NotificationCollectionChangedEventArgs(NotificationCollectionChangedAction.Add, notification));
        }

        private void Notification_OnReadStatusChanged(object sender, EventArgs e)
        {
            var notif = (sender as MenuNotificationViewModel).MenuNotification;

            if (repository.Contains(notif))
            {
                repository.Remove(notif);
            }

            NotificationCollectionChanged?.Invoke(this,
                new NotificationCollectionChangedEventArgs(NotificationCollectionChangedAction.ReadStatusChange, notif));
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
            var notifVms = (repository.Get().Select(n => new MenuNotificationViewModel(n)));
            foreach (var notifVm in notifVms)
            {
                notifVm.RequestClose += (s, e) => Remove(s as MenuNotificationViewModel);
                notifVm.Notification_ReadStatusChanged += Notification_OnReadStatusChanged;

                notifications.Add(notifVm);
            }

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
                .Where(n => !n.IsRead && !repository.Contains(n.MenuNotification))
                .ToArray();

            //List<MenuNotification> sNotifs = notifications.Where(u => u.IsDirty).ToList();

            foreach (var notif in sNotifications)
            {
                repository.Add(notif.MenuNotification);
            }            
        }

        #endregion // Public Methods
    }
}
