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
using System.Net;
using System.Threading.Tasks;

using Windows.UI.ViewManagement;
using Windows.Foundation;
using Windows.Storage;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;

using Inventory.Views;
using Inventory.ViewModels;
using Inventory.Services;

namespace Inventory
{
    static public class Startup
    {
        static private readonly ServiceCollection _serviceCollection = new ServiceCollection();

        static public async Task ConfigureAsync()
        {
            AppCenter.Start("7b48b5c7-768f-49e3-a2e4-7293abe8b0ca", typeof(Analytics), typeof(Crashes));
            Analytics.TrackEvent("AppStarted");

            ServiceLocator.Configure(_serviceCollection);

            ConfigureNavigation();

            await EnsureLogDbAsync();
            await EnsureDatabaseAsync();
            await ConfigureLookupTables();

            var logService = ServiceLocator.Current.GetService<ILogService>();
            await logService.WriteAsync(Data.LogType.Information, "Startup", "Configuration", "Application Start", $"Application started.");

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
        }

        private static void ConfigureNavigation()
        {
            NavigationService.Register<LoginViewModel, LoginView>();

            NavigationService.Register<ShellViewModel, ShellView>();
            NavigationService.Register<MainShellViewModel, MainShellView>();

            NavigationService.Register<DashboardViewModel, DashboardView>();

            NavigationService.Register<CustomersViewModel, CustomersView>();
            NavigationService.Register<CustomerDetailsViewModel, CustomerView>();

            NavigationService.Register<OrdersViewModel, OrdersView>();
            NavigationService.Register<OrderDetailsViewModel, OrderView>();

            NavigationService.Register<OrderItemsViewModel, OrderItemsView>();
            NavigationService.Register<OrderItemDetailsViewModel, OrderItemView>();

            NavigationService.Register<ProductsViewModel, ProductsView>();
            NavigationService.Register<ProductDetailsViewModel, ProductView>();

            NavigationService.Register<AppLogsViewModel, AppLogsView>();

            NavigationService.Register<SettingsViewModel, SettingsView>();
        }

        static private async Task EnsureLogDbAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var appLogFolder = await localFolder.CreateFolderAsync(AppSettings.AppLogPath, CreationCollisionOption.OpenIfExists);
            if (await appLogFolder.TryGetItemAsync(AppSettings.AppLogName) == null)
            {
                var sourceLogFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/AppLog/AppLog.db"));
                var targetLogFile = await appLogFolder.CreateFileAsync(AppSettings.AppLogName, CreationCollisionOption.ReplaceExisting);
                await sourceLogFile.CopyAndReplaceAsync(targetLogFile);
            }
        }

        static private async Task EnsureDatabaseAsync()
        {
            await EnsureSQLiteDatabaseAsync();
        }

        private static async Task EnsureSQLiteDatabaseAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var databaseFolder = await localFolder.CreateFolderAsync(AppSettings.DatabasePath, CreationCollisionOption.OpenIfExists);

            if (await databaseFolder.TryGetItemAsync(AppSettings.DatabaseName) == null)
            {
                if (await databaseFolder.TryGetItemAsync(AppSettings.DatabasePattern) == null)
                {
                    using (var cli = new WebClient())
                    {
                        var bytes = await Task.Run(() => cli.DownloadData(AppSettings.DatabaseUrl));
                        var file = await databaseFolder.CreateFileAsync(AppSettings.DatabasePattern, CreationCollisionOption.ReplaceExisting);
                        using (var stream = await file.OpenStreamForWriteAsync())
                        {
                            await stream.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                }
                var sourceFile = await databaseFolder.GetFileAsync(AppSettings.DatabasePattern);
                var targetFile = await databaseFolder.CreateFileAsync(AppSettings.DatabaseName, CreationCollisionOption.ReplaceExisting);
                await sourceFile.CopyAndReplaceAsync(targetFile);
            }
        }

        static private async Task ConfigureLookupTables()
        {
            var lookupTables = ServiceLocator.Current.GetService<ILookupTables>();
            await lookupTables.InitializeAsync();
            LookupTablesProxy.Instance = lookupTables;
        }
    }
}
