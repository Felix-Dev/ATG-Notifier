using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATG_Notifier.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ATG_Notifier.Data.DataContexts
{
    public class SQLiteDb : DbContext, IDataSource
    {
        private readonly string connectionString;

        public SQLiteDb(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($@"Data Source={connectionString}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangedNotifications);
        }

        public DbSet<DbVersion> DbVersion { get; set; }

        public DbSet<ChapterProfile> ChapterProfiles { get; set; }
    }
}
