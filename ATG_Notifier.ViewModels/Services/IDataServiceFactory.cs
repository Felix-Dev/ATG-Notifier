using ATG_Notifier.Data.Services;
using System.Threading.Tasks;

namespace ATG_Notifier.ViewModels.Services
{
    public interface IDataServiceFactory
    {
        Task<IDataService> CreateDataServiceAsync();
    }
}
