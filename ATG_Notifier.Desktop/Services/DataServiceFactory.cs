using ATG_Notifier.Data.Services;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services
{
    public class DataServiceFactory : IDataServiceFactory
    {
        public IDataService CreateDataService()
        {
            return new SQLiteDataService(AppConfiguration.DatabasePath);
        }
    }
}
