using ATG_Notifier.Data.Services;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.ViewModels.Services;
using System.IO;

namespace ATG_Notifier.Desktop.Services
{
    internal class DataServiceFactory : IDataServiceFactory
    {
        public IDataService CreateDataService()
        {
            // Needed because EF Core Sqlite Provider cannot create folders
            if (!Directory.Exists(AppConfiguration.DatabaseDirectory))
            {
                Directory.CreateDirectory(AppConfiguration.DatabaseDirectory);
            }

            return new SQLiteDataService(AppConfiguration.DatabasePath);
        }
    }
}
