#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

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
        public CustomerDetailsViewModel(ICustomerService customerService, IFilePickerService filePickerService, ICommonServices commonServices) : base(commonServices)
        {
            CustomerService = customerService;
            FilePickerService = filePickerService;
        }

        public ICustomerService CustomerService { get; }
        public IFilePickerService FilePickerService { get; }

        override public string Title => (Item?.IsNew ?? true) ? "New Customer" : TitleEdit;
        public string TitleEdit => Item == null ? "Customer" : $"{Item.FullName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

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
                try
                {
                    var item = await CustomerService.GetCustomerAsync(ViewModelArgs.CustomerID);
                    Item = item ?? new CustomerModel { CustomerID = ViewModelArgs.CustomerID, IsEmpty = true };
                }
                catch (Exception ex)
                {
                    LogException("Customer", "Load", ex);
                }
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

        private object _newPictureSource = null;
        public object NewPictureSource
        {
            get => _newPictureSource;
            set => Set(ref _newPictureSource, value);
        }

        public override void BeginEdit()
        {
            NewPictureSource = null;
            base.BeginEdit();
        }

        public ICommand EditPictureCommand => new RelayCommand(OnEditPicture);
        private async void OnEditPicture()
        {
            NewPictureSource = null;
            var result = await FilePickerService.OpenImagePickerAsync();
            if (result != null)
            {
                EditableItem.Picture = result.ImageBytes;
                EditableItem.PictureSource = result.ImageSource;
                EditableItem.Thumbnail = result.ImageBytes;
                EditableItem.ThumbnailSource = result.ImageSource;
                NewPictureSource = result.ImageSource;
            }
            else
            {
                NewPictureSource = null;
            }
        }

        protected override async Task<bool> SaveItemAsync(CustomerModel model)
        {
            try
            {
                StartStatusMessage("Saving customer...");
                await Task.Delay(100);
                await CustomerService.UpdateCustomerAsync(model);
                EndStatusMessage("Customer saved");
                LogInformation("Customer", "Save", "Customer saved successfully", $"Customer {model.CustomerID} '{model.FullName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Customer: {ex.Message}");
                LogException("Customer", "Save", ex);
                return false;
            }
        }

        protected override async Task<bool> DeleteItemAsync(CustomerModel model)
        {
            try
            {
                StartStatusMessage("Deleting customer...");
                await Task.Delay(100);
                await CustomerService.DeleteCustomerAsync(model);
                EndStatusMessage("Customer deleted");
                LogWarning("Customer", "Delete", "Customer deleted", $"Customer {model.CustomerID} '{model.FullName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Customer: {ex.Message}");
                LogException("Customer", "Delete", ex);
                return false;
            }
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
                                try
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
                                }
                                catch (Exception ex)
                                {
                                    LogException("Customer", "Handle Changes", ex);
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
                        try
                        {
                            var model = await CustomerService.GetCustomerAsync(current.CustomerID);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Customer", "Handle Ranges Deleted", ex);
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
