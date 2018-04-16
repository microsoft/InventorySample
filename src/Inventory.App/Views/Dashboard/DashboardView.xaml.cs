using System;

using Windows.UI.Xaml.Controls;

using Windows.UI.Xaml.Navigation;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class DashboardView : Page
    {
        public DashboardView()
        {
            ViewModel = ServiceLocator.Current.GetService<DashboardViewModel>();
            InitializeComponent();
        }

        public DashboardViewModel ViewModel { get; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unload();
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Control control)
            {
                ViewModel.ItemSelected(control.Tag as String);
            }
        }
    }
}
