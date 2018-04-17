using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region OrderItemDetailsArgs
    public class OrderItemDetailsArgs
    {
        static public OrderItemDetailsArgs CreateDefault() => new OrderItemDetailsArgs();

        public long OrderID { get; set; }
        public int OrderLine { get; set; }

        public bool IsNew => OrderLine <= 0;
    }
    #endregion

    public class OrderItemDetailsViewModel : GenericDetailsViewModel<OrderItemModel>
    {
        public OrderItemDetailsViewModel(IOrderItemService orderItemService, ICommonServices commonServices) : base(commonServices)
        {
            OrderItemService = orderItemService;
        }

        public IOrderItemService OrderItemService { get; }

        override public string Title => (Item?.IsNew ?? true) ? TitleNew : TitleEdit;
        public string TitleNew => $"New Order Item, Order #{OrderID}";
        public string TitleEdit => $"Order Line {Item?.OrderLine}, #{Item?.OrderID}" ?? String.Empty;

        public OrderItemDetailsArgs ViewModelArgs { get; private set; }

        public long OrderID { get; set; }

        public ICommand ProductSelectedCommand => new RelayCommand<ProductModel>(ProductSelected);
        private void ProductSelected(ProductModel product)
        {
            Item.ProductID = product.ProductID;
            Item.UnitPrice = product.ListPrice;
            Item.Product = product;

            Item.NotifyChanges();
        }

        public async Task LoadAsync(OrderItemDetailsArgs args)
        {
            ViewModelArgs = args ?? OrderItemDetailsArgs.CreateDefault();
            OrderID = ViewModelArgs.OrderID;

            if (ViewModelArgs.IsNew)
            {
                Item = new OrderItemModel { OrderID = OrderID };
                IsEditMode = true;
            }
            else
            {
                var item = await OrderItemService.GetOrderItemAsync(OrderID, ViewModelArgs.OrderLine);
                Item = item ?? new OrderItemModel { OrderID = OrderID, OrderLine = ViewModelArgs.OrderLine, IsEmpty = true };
            }
        }
        public void Unload()
        {
            ViewModelArgs.OrderID = Item?.OrderID ?? 0;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderItemDetailsViewModel, OrderItemModel>(this, OnDetailsMessage);
            MessageService.Subscribe<OrderItemListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public OrderItemDetailsArgs CreateArgs()
        {
            return new OrderItemDetailsArgs
            {
                OrderID = Item?.OrderID ?? 0,
                OrderLine = Item?.OrderLine ?? 0
            };
        }

        protected override async Task SaveItemAsync(OrderItemModel model)
        {
            StartStatusMessage("Saving order item...");
            await Task.Delay(100);
            await OrderItemService.UpdateOrderItemAsync(model);
            NotifyPropertyChanged(nameof(Title));
            EndStatusMessage("Order item saved");
        }

        protected override async Task DeleteItemAsync(OrderItemModel model)
        {
            StartStatusMessage("Deleting order item...");
            await Task.Delay(100);
            await OrderItemService.DeleteOrderItemAsync(model);
            EndStatusMessage("Order item deleted");
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current order item?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<OrderItemModel>> GetValidationConstraints(OrderItemModel model)
        {
            yield return new RequiredConstraint<OrderItemModel>("Product", m => m.ProductID);
            yield return new NonZeroConstraint<OrderItemModel>("Quantity", m => m.Quantity);
            yield return new PositiveConstraint<OrderItemModel>("Quantity", m => m.Quantity);
            yield return new LessThanConstraint<OrderItemModel>("Quantity", m => m.Quantity, 100);
            yield return new PositiveConstraint<OrderItemModel>("Discount", m => m.Discount);
            yield return new NonGreaterThanConstraint<OrderItemModel>("Discount", m => m.Discount, (double)model.Subtotal, "'Subtotal'");
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(OrderItemDetailsViewModel sender, string message, OrderItemModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.OrderID == current?.OrderID && changed.OrderLine == current?.OrderLine)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                var item = await OrderItemService.GetOrderItemAsync(current.OrderID, current.OrderLine);
                                item = item ?? new OrderItemModel { OrderID = OrderID, OrderLine = ViewModelArgs.OrderLine, IsEmpty = true };
                                current.Merge(item);
                                current.NotifyChanges();
                                if (IsEditMode)
                                {
                                    StatusMessage("WARNING: This orderItem has been modified externally");
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

        private async void OnListMessage(OrderItemListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<OrderItemModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.OrderID == current.OrderID && r.OrderLine == current.OrderLine))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        var model = await OrderItemService.GetOrderItemAsync(current.OrderID, current.OrderLine);
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
                StatusMessage("WARNING: This orderItem has been deleted externally");
            });
        }
    }
}
