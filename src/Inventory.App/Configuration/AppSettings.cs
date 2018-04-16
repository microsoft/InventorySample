using System;
using System.IO;

using Windows.Storage;
using Windows.ApplicationModel;

namespace Inventory
{
    public enum DataProviderType
    {
        SQLite,
        SQLServer,
        WebAPI
    }

    public class AppSettings
    {
        const string DB_NAME = "VanArsdel";
        const string DB_VERSION = "0.20";
        const string DB_BASEURL = "https://vanarsdelinventory.blob.core.windows.net/database";

        static AppSettings()
        {
            Current = new AppSettings();
        }

        static public AppSettings Current { get; }

        static public readonly string DatabasePath = "Database";
        static public readonly string DatabaseName = $"{DB_NAME}.{DB_VERSION}.db";
        static public readonly string DatabasePattern = $"{DB_NAME}.{DB_VERSION}.pattern.db";
        static public readonly string DatabaseFileName = Path.Combine(DatabasePath, DatabaseName);
        static public readonly string DatabaseUrl = $"{DB_BASEURL}/{DatabaseName}";

        public readonly string SQLiteConnectionString = $"Data Source={DatabaseFileName}";

        public ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

        public string Version
        {
            get
            {
                var ver = Package.Current.Id.Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
            }
        }

        public DataProviderType DataProvider
        {
            get => (DataProviderType)GetSettingsValue("DataProvider", (int)DataProviderType.SQLite);
            set => LocalSettings.Values["DataProvider"] = (int)value;
        }

        public string SQLServerConnectionString
        {
            get => GetSettingsValue("SQLServerConnectionString", @"Data Source=.\SQLExpress;Initial Catalog=VanArsdelDb2;Integrated Security=SSPI");
            set => SetSettingsValue("SQLServerConnectionString", value);
        }

        private TResult GetSettingsValue<TResult>(string name, TResult defaultValue)
        {
            try
            {
                if (!LocalSettings.Values.ContainsKey(name))
                {
                    LocalSettings.Values[name] = defaultValue;
                }
                return (TResult)LocalSettings.Values[name];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }
        private void SetSettingsValue(string name, object value)
        {
            LocalSettings.Values[name] = value;
        }
    }
}
