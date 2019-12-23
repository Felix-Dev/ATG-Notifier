using System;
using System.IO;
using System.Reflection;
using Windows.Storage;

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
        public static readonly string AppVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "ERROR";

        /// <summary>The database's file name.</summary>
        private static readonly string fullDatabaseName = $"{databaseName}.{databaseVersion}.db";

#if DESKTOPPACKAGE
        /// <summary>The path to the directory all app files will be written to.</summary>
        public static string BaseDirectory { get; } = ApplicationData.Current.LocalFolder.Path;
#else
        /// <summary>The path to the directory all app files will be written to.</summary>
        public static string BaseDirectory { get; } = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + $@"\{AppId}\");
#endif

        /// <summary>The path to the database file directory.</summary>
        public static string DatabaseDirectory { get; } = BaseDirectory;

        /// <summary>The path to the database file.</summary>
        public static string DatabasePath { get; } = Path.Combine(DatabaseDirectory, fullDatabaseName);

        /// <summary>The path to the logfile directory.</summary>
        public static string LogfileDirectory { get; } = Path.Combine(BaseDirectory, @"Logs\");

        /// <summary>The path to the logfile.</summary>
        public static string LogfilePath { get; } = Path.Combine(LogfileDirectory, $"{logFileName}");
    }
}
