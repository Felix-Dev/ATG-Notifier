using ATG_Notifier.Desktop.Helpers;
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

        /// <summary>The database's file name.</summary>
        private static readonly string fullDatabaseName = $"{databaseName}.{databaseVersion}.db";

        /// <summary>The name of the log file.</summary>
        private const string logFileName = "notifier.log";

        /// <summary>The relative path where app data will be stored to.</summary>
        private static string? appDataRelativePath = null;

        /// <summary>The relative path where app data will be stored to.</summary>
        private static string AppDataRelativePath => appDataRelativePath ??= GetAppDataRelativePath();

        /// <summary>The name of the app. </summary>
        public static readonly string AppId = System.Windows.Forms.Application.ProductName;

        /// <summary>The current app version. </summary>
        public static readonly string AppVersion = System.Windows.Forms.Application.ProductVersion;

        /// <summary>The link to the app's source code.</summary>
        public const string SourceCodeUri = "https://github.com/Felix-Dev/ATG-Notifier";

        /// <summary>The link to the app's feedback hub.</summary>
        public const string FeedbackUri = "https://github.com/Felix-Dev/ATG-Notifier/issues";

        /// <summary>The path to the configuration files directory.</summary>
        private static string? configurationDirectory = null;

        /// <summary>The path to the configuration files directory.</summary>
        public static string ConfigurationDirectory => configurationDirectory ??= GetConfigurationFilesDirectoryPath();

        /// <summary>The path to the database file directory.</summary>
        private static string? databaseDirectoy = null;

        /// <summary>The path to the database directory.</summary>
        public static string DatabaseDirectory => databaseDirectoy ??= GetDatabaseDirectoryPath();

        /// <summary>The path to the database file.</summary>
        private static string? databasePath = null;

        /// <summary>The path to the database file.</summary>
        public static string DatabasePath => databasePath ??= Path.Combine(DatabaseDirectory, fullDatabaseName);

        /// <summary>The path to the logfile directory.</summary>
        private static string? logfileDirectory = null;

        /// <summary>The path to the logfile directory.</summary>
        public static string LogfileDirectory => logfileDirectory ??= GetLogFilesDirectoryPath();

        /// <summary>The path to the logfile.</summary>
        private static string? logFilePath = null;

        /// <summary>The path to the logfile.</summary>
        public static string LogfilePath => logFilePath ??= Path.Combine(LogfileDirectory, logFileName);

        private static string GetConfigurationFilesDirectoryPath()
        {
            string basePath;
#if DesktopPackage || DesktopPackageDebug
            basePath = ApplicationData.Current.LocalFolder.Path;
#else
            basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataRelativePath);
#endif
            return Path.Combine(basePath, @"Configuration\");
        }

        private static string GetDatabaseDirectoryPath()
        {
            string basePath;
#if DesktopPackage || DesktopPackageDebug
            basePath = ApplicationData.Current.LocalFolder.Path;
#else
            basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataRelativePath);
#endif
            return Path.Combine(basePath, @"Data\");
        }

        private static string GetLogFilesDirectoryPath()
        {
            string basePath;
#if DesktopPackage || DesktopPackageDebug
            basePath = ApplicationData.Current.LocalCacheFolder.Path;
#else
            basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataRelativePath);
#endif
            return Path.Combine(basePath, @"Logs\");
        }

        private static string GetAppDataRelativePath()
        {
            // We attempt to create the following path:
            //
            //  %LOCALAPPDATA%/CompanyName/ProductName/  (*)
            //
            // (*) [CompanyName] is optional.

            string relativePath = "";
            string companyName = System.Windows.Forms.Application.CompanyName;
            string productName = AppConfiguration.AppId;

            if (!string.IsNullOrWhiteSpace(companyName))
            {
                relativePath = companyName;
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new InvalidOperationException("Could not create an app-specific base directory. Required [product name] could not be obtained.");
            }

            return Path.Combine(relativePath, productName);
        }
    }
}
