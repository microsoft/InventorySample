using System;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrderItemsViewModel : ViewModelBase
    {
        public OrderItemsViewModel(IOrderItemService orderItemService, IOrderService orderService, ICommonServices commonServices) : base(commonServices)
        {
            OrderItemService = orderItemService;

            OrderItemList = new OrderItemListViewModel(OrderItemService, commonServices);
            OrderItemDetails = new OrderItemDetailsViewModel(OrderItemService, commonServices);
        }

        public IOrderItemService OrderItemService { get; }

        public OrderItemListViewModel OrderItemList { get; set; }
        public OrderItemDetailsViewModel OrderItemDetails { get; set; }

        public async Task LoadAsync(OrderItemListArgs args)
        {
            await OrderItemList.LoadAsync(args);
        }
        public void Unload()
        {
            OrderItemDetails.CancelEdit();
            OrderItemList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderItemListViewModel>(this, OnMessage);
            OrderItemList.Subscribe();
            OrderItemDetails.Subscribe();
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            OrderItemList.Unsubscribe();
            OrderItemDetails.Unsubscribe();
        }

        private async void OnMessage(OrderItemListViewModel viewModel, string message, object args)
        {
            if (viewModel == OrderItemList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (OrderItemDetails.IsEditMode)
            {
                StatusReady();
                OrderItemDetails.CancelEdit();
            }
            var selected = OrderItemList.SelectedItem;
            if (!OrderItemList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            OrderItemDetails.Item = selected;
        }

        private async Task PopulateDetails(OrderItemModel selected)
        {
            var model = await OrderItemService.GetOrderItemAsync(selected.OrderID, selected.OrderLine);
            selected.Merge(model);
        }
    }
}
