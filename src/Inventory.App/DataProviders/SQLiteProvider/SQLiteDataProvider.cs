using System;

using Inventory.Data.Services;

namespace Inventory.Providers
{
    public class SQLiteDataProvider : SQLBaseProvider
    {
        public SQLiteDataProvider(string connectionString)
            : base(new SQLiteDataService(connectionString))
        {
        }
    }
}
