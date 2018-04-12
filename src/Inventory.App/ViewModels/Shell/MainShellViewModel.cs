using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Services;
using Inventory.Providers;
using Windows.UI.Xaml.Controls;

namespace Inventory.ViewModels
{
    public class MainShellViewModel : ShellViewModel
    {
        private readonly NavigationItem DashboardItem = new NavigationItem(0xE80F, "Dashboard", typeof(DashboardViewModel));
        private readonly NavigationItem CustomersItem = new NavigationItem(0xE716, "Customers", typeof(CustomersViewModel));
        private readonly NavigationItem OrdersItem = new NavigationItem(0xE14C, "Orders", typeof(OrdersViewModel));
        private readonly NavigationItem ProductsItem = new NavigationItem(0xECAA, "Products", typeof(ProductsViewModel));
        private readonly NavigationItem SettingsItem = new NavigationItem(0x0000, "Settings", typeof(SettingsViewModel));

        public MainShellViewModel(IDataProviderFactory providerFactory, INavigationService navigationService) : base(providerFactory, navigationService)
        {
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private bool _isPaneOpen = true;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => Set(ref _isPaneOpen, value);
        }

        private IEnumerable<NavigationItem> _items;
        public IEnumerable<NavigationItem> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        public override Task LoadAsync(ShellViewState viewState)
        {
            Items = GetItems().ToArray();
            return base.LoadAsync(viewState);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public void NavigateTo(Type viewModel)
        {
            switch (viewModel.Name)
            {
                case "DashboardViewModel":
                    NavigationService.Navigate(viewModel, new DashboardViewState());
                    break;
                case "CustomersViewModel":
                    NavigationService.Navigate(viewModel, new CustomersViewState());
                    break;
                case "OrdersViewModel":
                    NavigationService.Navigate(viewModel, new OrdersViewState());
                    break;
                case "ProductsViewModel":
                    NavigationService.Navigate(viewModel, new ProductsViewState());
                    break;
                case "SettingsViewModel":
                    NavigationService.Navigate(viewModel, new SettingsViewState());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<NavigationItem> GetItems()
        {
            yield return DashboardItem;
            yield return CustomersItem;
            yield return OrdersItem;
            yield return ProductsItem;
        }
    }
}
