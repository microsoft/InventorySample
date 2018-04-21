using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region CustomerListArgs
    public class CustomerListArgs
    {
        static public CustomerListArgs CreateEmpty() => new CustomerListArgs { IsEmpty = true };

        public CustomerListArgs()
        {
            OrderBy = r => r.FirstName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Customer, object>> OrderBy { get; set; }
        public Expression<Func<Customer, object>> OrderByDesc { get; set; }
    }
    #endregion

    public class CustomerListViewModel : GenericListViewModel<CustomerModel>
    {
        public CustomerListViewModel(ICustomerService customerService, ICommonServices commonServices) : base(commonServices)
        {
            CustomerService = customerService;
        }

        public ICustomerService CustomerService { get; }

        public CustomerListArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(CustomerListArgs args)
        {
            ViewModelArgs = args ?? CustomerListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading customers...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Customers loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CustomerListViewModel>(this, OnMessage);
            MessageService.Subscribe<CustomerDetailsViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public CustomerListArgs CreateArgs()
        {
            return new CustomerListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            Items = null;
            ItemsCount = 0;
            SelectedItem = null;

            try
            {
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<CustomerModel>();
                StatusError($"Error loading Customers: {ex.Message}");
                LogException("Customers", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<CustomerModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Customer> request = BuildDataRequest();
                return await CustomerService.GetCustomersAsync(request);
            }
            return new List<CustomerModel>();
        }

        protected override async void OnNew()
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<CustomerDetailsViewModel>(new CustomerDetailsArgs());
            }
            else
            {
                NavigationService.Navigate<CustomerDetailsViewModel>(new CustomerDetailsArgs());
            }

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading customers...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Customers loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected customers?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} customers...");
                        await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} customers...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                    EndStatusMessage($"{count} customers deleted");
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Customers: {ex.Message}");
                    LogException("Customers", "Delete", ex);
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<CustomerModel> models)
        {
            foreach (var model in models)
            {
                await CustomerService.DeleteCustomerAsync(model);
            }
        }

        private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        {
            DataRequest<Customer> request = BuildDataRequest();
            foreach (var range in ranges)
            {
                await CustomerService.DeleteCustomerRangeAsync(range.Index, range.Length, request);
            }
        }

        private DataRequest<Customer> BuildDataRequest()
        {
            return new DataRequest<Customer>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            switch (message)
            {
                case "ItemChanged":
                case "ItemDeleted":
                case "ItemsDeleted":
                case "ItemRangesDeleted":
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }
    }
}
