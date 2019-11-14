using ATG_Notifier.Data.DataContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Data.Services
{
    public class SQLiteDataService : DataServiceBase
    {
        public SQLiteDataService(string connectionString)
               : base(new SQLiteDb(connectionString))
        {
            using (var db = new SQLiteDb(connectionString))
            {
                db.Database.EnsureCreated();
            }
        }

    }
}
