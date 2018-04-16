using System;

using Inventory.Data.Services;

namespace Inventory.Services
{
    public class DataServiceFactory : IDataServiceFactory
    {
        public IDataService CreateDataService()
        {
            switch (AppSettings.Current.DataProvider)
            {
                case DataProviderType.SQLite:
                    return new SQLiteDataService(AppSettings.Current.SQLiteConnectionString);

                case DataProviderType.SQLServer:
                    return new SQLiteDataService(AppSettings.Current.SQLServerConnectionString);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
