using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;

using Inventory.ViewModels;
using Inventory.Services;

namespace Inventory.Views
{
    public sealed partial class ShellView : Page
    {
        public ShellView()
        {
            ViewModel = ServiceLocator.Current.GetService<ShellViewModel>();
            InitializeContext();
            InitializeComponent();
            InitializeNavigation();
        }

        public ShellViewModel ViewModel { get; private set; }

        private void InitializeContext()
        {
            var context = ServiceLocator.Current.GetService<IContextService>();
            context.Initialize(Dispatcher, ApplicationView.GetForCurrentView().Id, CoreApplication.GetCurrentView().IsMain);
        }

        private void InitializeNavigation()
        {
            var navigationService = ServiceLocator.Current.GetService<INavigationService>();
            navigationService.Initialize(frame);
            var appView = ApplicationView.GetForCurrentView();
            appView.Consolidated += OnViewConsolidated;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAsync(e.Parameter as ShellArgs);
            ViewModel.Subscribe();
        }

        private void OnViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            ViewModel.Unsubscribe();
            ViewModel = null;
            Bindings.StopTracking();
            frame.Navigate(typeof(Page));
            var appView = ApplicationView.GetForCurrentView();
            appView.Consolidated -= OnViewConsolidated;
            ServiceLocator.DisposeCurrent();
        }
    }
}
