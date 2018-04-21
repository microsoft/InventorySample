using System;

using Inventory.Data.Services;

namespace Inventory.Services
{
    public class DataServiceFactory : IDataServiceFactory
    {
        static private Random _random = new Random(0);

        public IDataService CreateDataService()
        {
            return new SQLiteDataService(AppSettings.Current.SQLiteConnectionString);
        }
    }
}
