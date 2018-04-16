using System;

using Inventory.Data.Services;

namespace Inventory.Services
{
    public interface IDataServiceFactory
    {
        IDataService CreateDataService();
    }
}
