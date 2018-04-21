using System;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public CustomersViewModel(ICustomerService customerService, IOrderService orderService, ICommonServices commonServices) : base(commonServices)
        {
            CustomerService = customerService;

            CustomerList = new CustomerListViewModel(CustomerService, commonServices);
            CustomerDetails = new CustomerDetailsViewModel(CustomerService, commonServices);
            CustomerOrders = new OrderListViewModel(orderService, commonServices);
        }

        public ICustomerService CustomerService { get; }

        public CustomerListViewModel CustomerList { get; set; }
        public CustomerDetailsViewModel CustomerDetails { get; set; }
        public OrderListViewModel CustomerOrders { get; set; }

        public async Task LoadAsync(CustomerListArgs args)
        {
            await CustomerList.LoadAsync(args);
        }
        public void Unload()
        {
            CustomerDetails.CancelEdit();
            CustomerList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CustomerListViewModel>(this, OnMessage);
            CustomerList.Subscribe();
            CustomerDetails.Subscribe();
            CustomerOrders.Subscribe();
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            CustomerList.Unsubscribe();
            CustomerDetails.Unsubscribe();
            CustomerOrders.Unsubscribe();
        }

        private async void OnMessage(CustomerListViewModel viewModel, string message, object args)
        {
            if (viewModel == CustomerList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (CustomerDetails.IsEditMode)
            {
                StatusReady();
                CustomerDetails.CancelEdit();
            }
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
        }

        private async Task PopulateDetails(CustomerModel selected)
        {
            try
            {
                var model = await CustomerService.GetCustomerAsync(selected.CustomerID);
                selected.Merge(model);
            }
            catch (Exception ex)
            {
                LogException("Customers",  "Load Details", ex);
            }
        }

        private async Task PopulateOrders(CustomerModel selectedItem)
        {
            try
            {
                if (selectedItem != null)
                {
                    await CustomerOrders.LoadAsync(new OrderListArgs { CustomerID = selectedItem.CustomerID }, silent: true);
                }
            }
            catch (Exception ex)
            {
                LogException("Customers", "Load Orders", ex);
            }
        }
    }
}
