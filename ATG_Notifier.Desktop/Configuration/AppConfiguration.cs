using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Configuration
{
    internal static class AppConfiguration
    {
        /// <summary>The name of the database.</summary>
        private const string databaseName = "Notifier";

        /// <summary>The version of the database.</summary>
        private const string databaseVersion = "1.10";

        /// <summary>The name of the log file.</summary>
        private const string logFileName = "notifier.log";

        /// <summary>The name of the app. </summary>
        public const string AppId = "ATG-Notifier";

        /// <summary>The current app version. </summary>
        public const string AppVersion = "1.4.0.0";

        /// <summary>The database's file name.</summary>
        private static readonly string fullDatabaseName = $"{databaseName}.{databaseVersion}.db";

        /// <summary>The path to the directory all app files will be written to.</summary>
        public static string BaseDirectory { get; } =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + @"\ATG_Notifier\");

        /// <summary>The path to the database file.</summary>
        public static string DatabasePath { get; } = Path.Combine(BaseDirectory, fullDatabaseName);

        /// <summary>The path to the logfile directory.</summary>
        public static string LogfileDirectory { get; } = Path.Combine(BaseDirectory, @"Logs\");

        /// <summary>The path to the logfile.</summary>
        public static string LogfilePath { get; } = Path.Combine(LogfileDirectory, $"{logFileName}");
    }
}
