using System;

namespace Inventory.Services
{
    public enum DataProviderType
    {
        SQLite,
        SQLServer,
        WebAPI
    }

    public interface ISettingsService
    {
        string Version { get; }

        DataProviderType DataProvider { get; set; }
        string SQLServerConnectionString { get; set; }
        bool IsRandomErrorsEnabled { get; set; }
    }
}
