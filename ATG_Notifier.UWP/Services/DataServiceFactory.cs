using ATG_Notifier.Data.Services;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Services
{
    public class DataServiceFactory : IDataServiceFactory
    {
        public IDataService CreateDataService()
        {
            return new SQLiteDataService(App.DatabaseConnectionString);
        }
    }
}
