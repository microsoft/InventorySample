using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region CustomerDetailsArgs
    public class CustomerDetailsArgs
    {
        static public CustomerDetailsArgs CreateDefault() => new CustomerDetailsArgs();

        public long CustomerID { get; set; }

        public bool IsNew => CustomerID <= 0;
    }
    #endregion

    public class CustomerDetailsViewModel : GenericDetailsViewModel<CustomerModel>
    {
        public CustomerDetailsViewModel(ICustomerService customerService, ICommonServices commonServices) : base(commonServices)
        {
            CustomerService = customerService;
        }

        public ICustomerService CustomerService { get; }

        override public string Title => (Item?.IsNew ?? true) ? "New Customer" : TitleEdit;
        public string TitleEdit => Item == null ? "Customer" : $"{Item.FullName}";

        public CustomerDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(CustomerDetailsArgs args)
        {
            ViewModelArgs = args ?? CustomerDetailsArgs.CreateDefault();

            if (ViewModelArgs.IsNew)
            {
                Item = new CustomerModel();
                IsEditMode = true;
            }
            else
            {
                var item = await CustomerService.GetCustomerAsync(ViewModelArgs.CustomerID);
                Item = item ?? new CustomerModel { CustomerID = ViewModelArgs.CustomerID, IsEmpty = true };
            }
        }
        public void Unload()
        {
            ViewModelArgs.CustomerID = Item?.CustomerID ?? 0;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CustomerDetailsViewModel, CustomerModel>(this, OnDetailsMessage);
            MessageService.Subscribe<CustomerListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public CustomerDetailsArgs CreateArgs()
        {
            return new CustomerDetailsArgs
            {
                CustomerID = Item?.CustomerID ?? 0
            };
        }

        protected override async Task SaveItemAsync(CustomerModel model)
        {
            StartStatusMessage("Saving customer...");
            await Task.Delay(100);
            await CustomerService.UpdateCustomerAsync(model);
            EndStatusMessage("Customer saved");
        }

        protected override async Task DeleteItemAsync(CustomerModel model)
        {
            StartStatusMessage("Deleting customer...");
            await Task.Delay(100);
            await CustomerService.DeleteCustomerAsync(model);
            EndStatusMessage("Customer deleted");
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current customer?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<CustomerModel>> GetValidationConstraints(CustomerModel model)
        {
            yield return new RequiredConstraint<CustomerModel>("First Name", m => m.FirstName);
            yield return new RequiredConstraint<CustomerModel>("Last Name", m => m.LastName);
            yield return new RequiredConstraint<CustomerModel>("Email Address", m => m.EmailAddress);
            yield return new RequiredConstraint<CustomerModel>("Address Line 1", m => m.AddressLine1);
            yield return new RequiredConstraint<CustomerModel>("City", m => m.City);
            yield return new RequiredConstraint<CustomerModel>("Region", m => m.Region);
            yield return new RequiredConstraint<CustomerModel>("Postal Code", m => m.PostalCode);
            yield return new RequiredConstraint<CustomerModel>("Country", m => m.CountryCode);
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(CustomerDetailsViewModel sender, string message, CustomerModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.CustomerID == current?.CustomerID)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                var item = await CustomerService.GetCustomerAsync(current.CustomerID);
                                item = item ?? new CustomerModel { CustomerID = current.CustomerID, IsEmpty = true };
                                current.Merge(item);
                                current.NotifyChanges();
                                NotifyPropertyChanged(nameof(Title));
                                if (IsEditMode)
                                {
                                    StatusMessage("WARNING: This customer has been modified externally");
                                }
                            });
                            break;
                        case "ItemDeleted":
                            await OnItemDeletedExternally();
                            break;
                    }
                }
            }
        }

        private async void OnListMessage(CustomerListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<CustomerModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.CustomerID == current.CustomerID))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        var model = await CustomerService.GetCustomerAsync(current.CustomerID);
                        if (model == null)
                        {
                            await OnItemDeletedExternally();
                        }
                        break;
                }
            }
        }

        private async Task OnItemDeletedExternally()
        {
            await ContextService.RunAsync(() =>
            {
                CancelEdit();
                IsEnabled = false;
                StatusMessage("WARNING: This customer has been deleted externally");
            });
        }
    }
}
