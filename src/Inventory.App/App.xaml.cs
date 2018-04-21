using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;

using Inventory.Views;
using Inventory.ViewModels;
using Inventory.Services;

namespace Inventory
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 840);
            this.Suspending += OnSuspending;
            this.UnhandledException += OnUnhandledException;
        }

        private Type EntryViewModel => typeof(DashboardViewModel);
        private object EntryArgs => null;

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;

            if (frame == null)
            {
                frame = new Frame();
                Window.Current.Content = frame;
            }

            if (e.PrelaunchActivated == false)
            {
                await Startup.ConfigureAsync();

                if (frame.Content == null)
                {
                    var args = new ShellArgs
                    {
                        ViewModel = EntryViewModel,
                        Parameter = EntryArgs
                    };
                    frame.Navigate(typeof(MainShellView), args);
                }
                Window.Current.Activate();
            }

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var logService = ServiceLocator.Current.GetService<ILogService>();
            await logService.WriteAsync(Data.LogType.Information, "App", "Suspending", "Application End", $"Application ended by [User].");
        }

        private void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            var logService = ServiceLocator.Current.GetService<ILogService>();
            logService.WriteAsync(Data.LogType.Error, "App", "UnhandledException", e.Message, e.Exception.ToString());
        }
    }
}
