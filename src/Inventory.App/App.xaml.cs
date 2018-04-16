using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;

using Inventory.Views;
using Inventory.ViewModels;

namespace Inventory
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 840);
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
    }
}
