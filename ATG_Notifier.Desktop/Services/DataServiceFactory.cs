using ATG_Notifier.Data.Services;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.ViewModels.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services
{
    internal class DataServiceFactory : IDataServiceFactory
    {
        public async Task<IDataService> CreateDataServiceAsync()
        {
            // The EF Core Sqlite Provider cannot create folders so we have to create any non-existing folders
            // in the database path before attempting to establish a connection to the database.
            if (!Directory.Exists(AppConfiguration.DatabaseDirectory))
            {
                Directory.CreateDirectory(AppConfiguration.DatabaseDirectory);
            }

            return await SQLiteDataService.CreateDataServiceAsync(AppConfiguration.DatabasePath).ConfigureAwait(false);
        }
    }
}
