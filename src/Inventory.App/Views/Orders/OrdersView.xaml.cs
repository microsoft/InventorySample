using System;
using System.ComponentModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Inventory.ViewModels;
using Inventory.Services;

namespace Inventory.Views
{
    public sealed partial class OrdersView : Page
    {
        public OrdersView()
        {
            ViewModel = ServiceLocator.Current.GetService<OrdersViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            InitializeComponent();
        }

        public OrdersViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.OrderList.PropertyChanged += OnViewModelPropertyChanged;
            await ViewModel.LoadAsync(e.Parameter as OrdersViewState);
            UpdateTitle();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.CancelEdit();
            ViewModel.Unload();
            ViewModel.OrderList.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.OrderList.Title))
            {
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            this.SetTitle($"Orders {ViewModel.OrderList.Title}".Trim());
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await ViewModel.RefreshAsync();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            await NavigationService.CreateNewViewAsync<OrdersViewModel>(ViewModel.OrderList.GetCurrentState());
        }

        private async void OpenDetailsInNewView(object sender, RoutedEventArgs e)
        {
            ViewModel.OrderDetails.IsEditMode = false;
            if (pivot.SelectedIndex == 0)
            {
                await NavigationService.CreateNewViewAsync<OrderDetailsViewModel>(new OrderViewState(ViewModel.OrderDetails.Item.CustomerID) { OrderID = ViewModel.OrderDetails.Item.OrderID });
            }
            else
            {
                await NavigationService.CreateNewViewAsync<OrderItemsViewModel>(ViewModel.OrderItemList.ViewState.Clone());
            }
        }

        public int GetRowSpan(bool isMultipleSelection)
        {
            return isMultipleSelection ? 2 : 1;
        }
    }
}
