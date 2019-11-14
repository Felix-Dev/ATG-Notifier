using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Configuration
{
    public static class AppConfiguration
    {
        /// <summary>The name of the database.</summary>
        private const string databaseName = "Notifier";

        /// <summary>The version of the database.</summary>
        private const string databaseVersion = "1.10";

        public const string AppId = "ATG-Notifier";

        /// <summary>The database's file name.</summary>
        private static readonly string fullDatabaseName = $"{databaseName}.{databaseVersion}.db";

        /// <summary>The path to the directory the database file will be written to.</summary>
        public static string DatabaseDirectory { get; } =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + @"\ATG_Notifier\");

        /// <summary>The path to the database file.</summary>
        public static string DatabasePath { get; } = Path.Combine(DatabaseDirectory, fullDatabaseName);
    }
}
