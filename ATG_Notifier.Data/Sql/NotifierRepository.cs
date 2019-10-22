using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ATG_Notifier.Data.Sql
{
    public class NotifierRepository : INotifierRepository
    {
        private readonly DbContextOptions<NotifierContext> dbOptions;

        public NotifierRepository(string path)
        {
            // Needed because EF Core Sqlite Provider cannot create folders
            var dir = path.Substring(0, path.LastIndexOf('\\') + 1);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            dbOptions = new DbContextOptionsBuilder<NotifierContext>().UseSqlite($"Data Source={path}").Options;

            using (var db = new NotifierContext(dbOptions))
            {
                db.Database.EnsureCreated();
            }
        }

        public INotificationRepository Notifications => new NotificationRepository(new NotifierContext(dbOptions));
    }
}
