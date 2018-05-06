#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.IO;

using Windows.Storage;
using Windows.ApplicationModel;

using Inventory.Services;

namespace Inventory
{
    public class AppSettings
    {
        const string DB_NAME = "VanArsdel";
        const string DB_VERSION = "1.01";
        const string DB_BASEURL = "https://vanarsdelinventory.blob.core.windows.net/database";

        static AppSettings()
        {
            Current = new AppSettings();
        }

        static public AppSettings Current { get; }

        static public readonly string AppLogPath = "AppLog";
        static public readonly string AppLogName = $"AppLog.1.0.db";
        static public readonly string AppLogFileName = Path.Combine(AppLogPath, AppLogName);

        public readonly string AppLogConnectionString = $"Data Source={AppLogFileName}";

        static public readonly string DatabasePath = "Database";
        static public readonly string DatabaseName = $"{DB_NAME}.{DB_VERSION}.db";
        static public readonly string DatabasePattern = $"{DB_NAME}.{DB_VERSION}.pattern.db";
        static public readonly string DatabaseFileName = Path.Combine(DatabasePath, DatabaseName);
        static public readonly string DatabasePatternFileName = Path.Combine(DatabasePath, DatabasePattern);
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

        public string DbVersion => DB_VERSION;

        public string UserName
        {
            get => GetSettingsValue("UserName", default(String));
            set => LocalSettings.Values["UserName"] = value;
        }

        public string WindowsHelloPublicKeyHint
        {
            get => GetSettingsValue("WindowsHelloPublicKeyHint", default(String));
            set => LocalSettings.Values["WindowsHelloPublicKeyHint"] = value;
        }

        public DataProviderType DataProvider
        {
            get => (DataProviderType)GetSettingsValue("DataProvider", (int)DataProviderType.SQLite);
            set => LocalSettings.Values["DataProvider"] = (int)value;
        }

        public string SQLServerConnectionString
        {
            get => GetSettingsValue("SQLServerConnectionString", @"Data Source=.\SQLExpress;Initial Catalog=VanArsdelDb;Integrated Security=SSPI");
            set => SetSettingsValue("SQLServerConnectionString", value);
        }

        public bool IsRandomErrorsEnabled
        {
            get => GetSettingsValue("IsRandomErrorsEnabled", false);
            set => LocalSettings.Values["IsRandomErrorsEnabled"] = value;
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
