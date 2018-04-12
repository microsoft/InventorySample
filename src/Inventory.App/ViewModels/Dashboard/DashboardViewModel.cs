using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(IDataProviderFactory providerFactory, INavigationService navigationService)
        {
            ProviderFactory = providerFactory;
            NavigationService = navigationService;
        }

        public IDataProviderFactory ProviderFactory { get; }
        public INavigationService NavigationService { get; }

        private IList<CustomerModel> _customers = null;
        public IList<CustomerModel> Customers
        {
            get => _customers;
            set => Set(ref _customers, value);
        }

        private IList<ProductModel> _products = null;
        public IList<ProductModel> Products
        {
            get => _products;
            set => Set(ref _products, value);
        }

        private IList<OrderModel> _orders = null;
        public IList<OrderModel> Orders
        {
            get => _orders;
            set => Set(ref _orders, value);
        }

        public void ItemSelected(string item)
        {
            switch (item)
            {
                case "Customers":
                    NavigationService.Navigate<CustomersViewModel>(new CustomersViewState { OrderByDesc = r => r.CreatedOn });
                    break;
                case "Orders":
                    NavigationService.Navigate<OrdersViewModel>(new OrdersViewState { OrderByDesc = r => r.OrderDate });
                    break;
                case "Products":
                    NavigationService.Navigate<ProductsViewModel>(new ProductsViewState { OrderByDesc = r => r.ListPrice });
                    break;
                default:
                    break;
            }
        }

        public async Task LoadAsync()
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await LoadCustomersAsync(dataProvider);
                await LoadProductsAsync(dataProvider);
                await LoadOrdersAsync(dataProvider);
            }
        }

        public void Unload()
        {
            Customers = null;
            Products = null;
            Orders = null;
        }

        private async Task LoadCustomersAsync(IDataProvider dataProvider)
        {
            var request = new DataRequest<Customer>
            {
                OrderByDesc = r => r.CreatedOn
            };
            Customers = await dataProvider.GetCustomersAsync(0, 5, request);
        }

        private async Task LoadProductsAsync(IDataProvider dataProvider)
        {
            var request = new DataRequest<Product>
            {
                OrderByDesc = r => r.CreatedOn
            };
            Products = await dataProvider.GetProductsAsync(0, 5, request);
        }

        private async Task LoadOrdersAsync(IDataProvider dataProvider)
        {
            var request = new DataRequest<Order>
            {
                OrderByDesc = r => r.OrderDate
            };
            Orders = await dataProvider.GetOrdersAsync(0, 5, request);
        }
    }
}
