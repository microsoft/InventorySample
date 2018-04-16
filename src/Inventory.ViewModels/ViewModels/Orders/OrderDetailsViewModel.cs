using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region OrderDetailsArgs
    public class OrderDetailsArgs
    {
        static public OrderDetailsArgs CreateDefault() => new OrderDetailsArgs { CustomerID = 0 };

        public long CustomerID { get; set; }
        public long OrderID { get; set; }

        public bool IsNew => OrderID <= 0;
    }
    #endregion

    public class OrderDetailsViewModel : GenericDetailsViewModel<OrderModel>
    {
        public OrderDetailsViewModel(IOrderService orderService, ICommonServices commonServices) : base(commonServices)
        {
            OrderService = orderService;
        }

        public IOrderService OrderService { get; }

        override public string Title => (Item?.IsNew ?? true) ? TitleNew : TitleEdit;
        public string TitleNew => Item?.Customer == null ? "New Order" : $"New Order, {Item?.Customer?.FullName}";
        public string TitleEdit => Item == null ? "Order" : $"Order #{Item?.OrderID}";

        public ICommand CustomerSelectedCommand => new RelayCommand<CustomerModel>(CustomerSelected);
        private void CustomerSelected(CustomerModel customer)
        {
            Item.CustomerID = customer.CustomerID;
            Item.ShipAddress = customer.AddressLine1;
            Item.ShipCity = customer.City;
            Item.ShipRegion = customer.Region;
            Item.ShipCountryCode = customer.CountryCode;
            Item.ShipPostalCode = customer.PostalCode;
            Item.Customer = customer;

            Item.NotifyChanges();
        }

        public OrderDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(OrderDetailsArgs args)
        {
            ViewModelArgs = args ?? OrderDetailsArgs.CreateDefault();

            if (ViewModelArgs.IsNew)
            {
                Item = await OrderService.CreateNewOrderAsync(ViewModelArgs.CustomerID);
                IsEditMode = true;
            }
            else
            {
                var item = await OrderService.GetOrderAsync(ViewModelArgs.OrderID);
                Item = item ?? new OrderModel { OrderID = ViewModelArgs.OrderID, IsEmpty = true };
            }
            if (Item != null)
            {
                Item.CanEditCustomer = args.CustomerID <= 0;
            }
        }
        public void Unload()
        {
            ViewModelArgs.CustomerID = Item?.CustomerID ?? 0;
            ViewModelArgs.OrderID = Item?.OrderID ?? 0;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderDetailsViewModel, OrderModel>(this, OnDetailsMessage);
            MessageService.Subscribe<OrderListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public OrderDetailsArgs CreateArgs()
        {
            return new OrderDetailsArgs
            {
                CustomerID = Item?.CustomerID ?? 0,
                OrderID = Item?.OrderID ?? 0
            };
        }

        protected override async Task SaveItemAsync(OrderModel model)
        {
            StartStatusMessage("Saving order...");
            await Task.Delay(100);
            await OrderService.UpdateOrderAsync(model);
            NotifyPropertyChanged(nameof(Title));
            EndStatusMessage("Order saved");
        }

        protected override async Task DeleteItemAsync(OrderModel model)
        {
            StartStatusMessage("Deleting order...");
            await Task.Delay(100);
            await OrderService.DeleteOrderAsync(model);
            EndStatusMessage("Order deleted");
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current order?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<OrderModel>> ValidationConstraints
        {
            get
            {
                yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Customer", m => m.CustomerID);
                if (Item.Status > 0)
                {
                    yield return new RequiredConstraint<OrderModel>("Payment Type", m => m.PaymentType);
                    yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Payment Type", m => m.PaymentType);
                    if (Item.Status > 1)
                    {
                        yield return new RequiredConstraint<OrderModel>("Shipper", m => m.ShipVia);
                        yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Shipper", m => m.ShipVia);
                    }
                }
            }
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(OrderDetailsViewModel sender, string message, OrderModel changed)
        {
            var current = ItemReadOnly;
            if (current != null)
            {
                if (changed != null && changed.OrderID == ItemReadOnly?.OrderID)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                var item = await OrderService.GetOrderAsync(current.OrderID);
                                item = item ?? new OrderModel { OrderID = current.OrderID, IsEmpty = true };
                                current.Merge(item);
                                current.NotifyChanges();
                                if (IsEditMode)
                                {
                                    StatusMessage("WARNING: This order has been modified externally");
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

        private async void OnListMessage(OrderListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<OrderModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.OrderID == Item.OrderID))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        var model = await OrderService.GetOrderAsync(Item.OrderID);
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
                StatusMessage("WARNING: This order has been deleted externally");
            });
        }
    }
}
