using ATG_Notifier.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ATG_Notifier.Data
{
    public class NotificationRepository : INotificationRepository
    {
        private NotifierContext db;

        #region Creation

        internal NotificationRepository(NotifierContext dbContext)
        {
            db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #endregion // Creation

        #region INotificationRepository Implementation

        public IQueryable<MenuNotification> Get()
        {
            return (from u in db.Notifications
                    select new MenuNotification()
                    {
                        Id = u.Id,
                        Title = u.Title,
                        Body = u.Body,
                        Url = u.Url,
                        ArrivalTime = u.ArrivalTime,
                        IsRead = u.IsRead,
                    });
        }

        public MenuNotification Get(int id)
        {
            return (from u in db.Notifications
                    where u.Id == id
                    select new MenuNotification()
                    {
                        Id = u.Id,
                        Title = u.Title,
                        Body = u.Body,
                        Url = u.Url,
                        ArrivalTime = u.ArrivalTime,
                        IsRead = u.IsRead,
                    }).FirstOrDefault();
        }

        public MenuNotification Add(MenuNotification notification)
        {
            var dbNotif =
                new ATG_Notifier.Data.Entities.MenuNotification()
                {
                    Title = notification.Title,
                    Body = notification.Body,
                    Url = notification.Url,
                    ArrivalTime = notification.ArrivalTime,
                    IsRead = notification.IsRead,
                };

            db.Notifications.Add(dbNotif);
            db.SaveChanges();

            notification.Id = dbNotif.Id;

            return notification;
        }

        public MenuNotification Update(MenuNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            throw new NotImplementedException();
        }

        public void Remove(MenuNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var dbNotif = db.Notifications
                .Where(u => u.Id == notification.Id)
                .FirstOrDefault();

            if (dbNotif != null)
            {
                db.Entry(dbNotif).State = EntityState.Deleted;
                db.SaveChanges();

                notification.Id = 0;
            }
        }

        public bool Contains(MenuNotification notification)
        {
            if (notification == null)
            {
                return false;
            }

            return db.Notifications.Any(n => n.Id == notification.Id);
        }

        #endregion // INotificationRepository Implementation
    }
}
