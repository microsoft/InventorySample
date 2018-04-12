using System;

using Inventory.Data.Services;

namespace Inventory.Providers
{
    public class SQLServerDataProvider : SQLBaseProvider
    {
        public SQLServerDataProvider(string connectionString)
            : base(new SQLServerDataService(connectionString))
        {
        }
    }
}
