using ATG_Notifier.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ATG_Notifier.Data
{
    internal class NotifierContext : DbContext
    {      
        public NotifierContext(DbContextOptions <NotifierContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite($@"Data Source={path}");
        }

        public DbSet<MenuNotification> Notifications { get; set; }
    }
}
