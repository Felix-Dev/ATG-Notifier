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
        public static readonly string AppId = System.Windows.Forms.Application.ProductName;

        /// <summary>The current app version. </summary>
        public static readonly string AppVersion = System.Windows.Forms.Application.ProductVersion;

        /// <summary>The link to the app's source code.</summary>
        public const string SourceCodeUri = "https://github.com/Felix-Dev/ATG-Notifier";

        /// <summary>The link to the app's feedback hub.</summary>
        public const string FeedbackUri = "https://github.com/Felix-Dev/ATG-Notifier/issues";

        /// <summary>The database's file name.</summary>
        private static readonly string fullDatabaseName = $"{databaseName}.{databaseVersion}.db";

        /// <summary>The path to the directory all app files will be written to.</summary>
        public static string BaseDirectory { get; } = GetBaseDirectory();

        /// <summary>The path to the configuration files directory.</summary>
        public static string ConfigurationDirectory { get; } = Path.Combine(BaseDirectory, @"Configuration\");

        /// <summary>The path to the database file directory.</summary>
        public static string DatabaseDirectory { get; } = Path.Combine(BaseDirectory, @"Data\");

        /// <summary>The path to the database file.</summary>
        public static string DatabasePath { get; } = Path.Combine(DatabaseDirectory, fullDatabaseName);

        /// <summary>The path to the logfile directory.</summary>
        public static string LogfileDirectory { get; } = Path.Combine(BaseDirectory, @"Logs\");

        /// <summary>The path to the logfile.</summary>
        public static string LogfilePath { get; } = Path.Combine(LogfileDirectory, $"{logFileName}");

        private static string GetBaseDirectory()
        {
#if DESKTOPPACKAGE
            return GetBaseDirectoryForPackagedApp();
#else
            return GetBaseDirectoryForUnpackagedApp();
#endif
        }

        private static string GetBaseDirectoryForUnpackagedApp()
        {
            // We attempt to create the following path:
            //
            //  %LOCALAPPDATA%/CompanyName/ProductName/  (*)
            //
            // (*) [CompanyName] is optional.
            string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string companyName = System.Windows.Forms.Application.CompanyName;
            string productName = AppConfiguration.AppId;

            if (!string.IsNullOrWhiteSpace(companyName))
            {
                baseDir = Path.Combine(baseDir, companyName);
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new InvalidOperationException("Could not create an app-specific base directory. Required [product name] could not be obtained.");
            }

            return Path.Combine(baseDir, productName);
        }

        private static string GetBaseDirectoryForPackagedApp()
        {
            return ApplicationData.Current.LocalFolder.Path;
        }
    }
}
