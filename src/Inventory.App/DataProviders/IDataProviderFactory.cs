using System;

namespace Inventory.Providers
{
    public interface IDataProviderFactory
    {
        IDataProvider CreateDataProvider();
    }
}
