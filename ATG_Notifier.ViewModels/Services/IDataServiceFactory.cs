using ATG_Notifier.Data.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Services
{
    public interface IDataServiceFactory
    {
        IDataService CreateDataService();
    }
}
