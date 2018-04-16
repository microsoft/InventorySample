using System;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        public OrdersViewModel(IOrderService orderService, IOrderItemService orderItemService, ICommonServices commonServices) : base(commonServices)
        {
            OrderService = orderService;

            OrderList = new OrderListViewModel(OrderService, commonServices);
            OrderDetails = new OrderDetailsViewModel(OrderService, commonServices);
            OrderItemList = new OrderItemListViewModel(orderItemService, commonServices);
        }

        public IOrderService OrderService { get; }

        public OrderListViewModel OrderList { get; set; }
        public OrderDetailsViewModel OrderDetails { get; set; }
        public OrderItemListViewModel OrderItemList { get; set; }

        public async Task LoadAsync(OrderListArgs args)
        {
            await OrderList.LoadAsync(args);
        }
        public void Unload()
        {
            OrderDetails.CancelEdit();
            OrderList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderListViewModel>(this, OnMessage);
            OrderList.Subscribe();
            OrderDetails.Subscribe();
            OrderItemList.Subscribe();
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            OrderList.Unsubscribe();
            OrderDetails.Unsubscribe();
            OrderItemList.Unsubscribe();
        }

        private async void OnMessage(OrderListViewModel viewModel, string message, object args)
        {
            if (viewModel == OrderList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (OrderDetails.IsEditMode)
            {
                StatusReady();
                OrderDetails.CancelEdit();
            }
            OrderItemList.IsMultipleSelection = false;
            var selected = OrderList.SelectedItem;
            if (!OrderList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                    await PopulateOrders(selected);
                }
            }
            OrderDetails.Item = selected;
        }

        private async Task PopulateDetails(OrderModel selected)
        {
            var model = await OrderService.GetOrderAsync(selected.OrderID);
            selected.Merge(model);
        }

        private async Task PopulateOrders(OrderModel selectedItem)
        {
            if (selectedItem != null)
            {
                await OrderItemList.LoadAsync(new OrderItemListArgs { OrderID = selectedItem.OrderID }, silent: true);
            }
        }
    }
}
