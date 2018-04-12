using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public CustomersViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
        {
            ProviderFactory = providerFactory;

            CustomerList = new CustomerListViewModel(ProviderFactory, serviceManager);
            CustomerList.PropertyChanged += OnListPropertyChanged;

            CustomerDetails = new CustomerDetailsViewModel(ProviderFactory, serviceManager);
            CustomerDetails.ItemDeleted += OnItemDeleted;

            CustomerOrders = new OrderListViewModel(ProviderFactory, serviceManager);
        }

        public IDataProviderFactory ProviderFactory { get; }

        public CustomerListViewModel CustomerList { get; set; }
        public CustomerDetailsViewModel CustomerDetails { get; set; }
        public OrderListViewModel CustomerOrders { get; set; }

        public async Task LoadAsync(CustomersViewState state)
        {
            await CustomerList.LoadAsync(state);
        }

        public void Unload()
        {
            CustomerList.Unload();
        }

        public async Task RefreshAsync()
        {
            await CustomerList.RefreshAsync();
        }

        private async void OnListPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CustomerListViewModel.SelectedItem):
                    CustomerDetails.CancelEdit();
                    CustomerOrders.IsMultipleSelection = false;
                    var selected = CustomerList.SelectedItem;
                    if (!CustomerList.IsMultipleSelection)
                    {
                        if (selected != null && !selected.IsEmpty)
                        {
                            await PopulateDetails(selected);
                            await PopulateOrders(selected);
                        }
                    }
                    CustomerDetails.Item = selected;
                    break;
                default:
                    break;
            }
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await CustomerList.RefreshAsync();
        }

        private async Task PopulateDetails(CustomerModel selected)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                var model = await dataProvider.GetCustomerAsync(selected.CustomerID);
                selected.Merge(model);
            }
        }

        private async Task PopulateOrders(CustomerModel selectedItem)
        {
            if (selectedItem != null)
            {
                await CustomerOrders.LoadAsync(new OrdersViewState { CustomerID = selectedItem.CustomerID });
            }
        }

        public void CancelEdit()
        {
            CustomerDetails.CancelEdit();
        }
    }
}
