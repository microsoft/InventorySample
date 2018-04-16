using System;

namespace Inventory.Data.Services
{
    public class SQLiteDataService : DataServiceBase
    {
        public SQLiteDataService(string connectionString)
            : base(new SQLiteDb(connectionString))
        {
        }
    }
}
