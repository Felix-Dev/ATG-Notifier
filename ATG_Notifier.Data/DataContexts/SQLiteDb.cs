using ATG_Notifier.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ATG_Notifier.Data.DataContexts
{
    public class SQLiteDb : DbContext, IDataSource
    {
        private readonly string connectionString;

        public SQLiteDb(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($@"Data Source={connectionString}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangedNotifications);
        }

        public int DeleteChapterProfiles()
        {
            return this.Database.ExecuteSqlRaw("DELETE FROM [ChapterProfiles]");
        }

        public DbSet<DbVersion> DbVersion { get; set; } = null!;

        public DbSet<ChapterProfile> ChapterProfiles { get; set; } = null!;
    }
}
