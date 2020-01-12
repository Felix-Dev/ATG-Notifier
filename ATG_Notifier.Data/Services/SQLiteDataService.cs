using ATG_Notifier.Data.DataContexts;
using System.Threading.Tasks;

namespace ATG_Notifier.Data.Services
{
    public class SQLiteDataService : DataServiceBase
    {
        private SQLiteDataService(SQLiteDb dataSource)
            : base(dataSource)
        {
            dataSource.Database.EnsureCreated();
        }

        public static async Task<SQLiteDataService> CreateDataServiceAsync(string connectionString)
        {
            return await Task.Run(() =>
            {
                var db = new SQLiteDb(connectionString);

                return new SQLiteDataService(db);
            }).ConfigureAwait(false);
            
            
        }
    }
}
