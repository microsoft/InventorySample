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
    #region OrderItemListArgs
    public class OrderItemListArgs
    {
        static public OrderItemListArgs CreateEmpty() => new OrderItemListArgs { IsEmpty = true };

        public OrderItemListArgs()
        {
            OrderBy = r => r.OrderLine;
        }

        public long OrderID { get; set; }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<OrderItem, object>> OrderBy { get; set; }
        public Expression<Func<OrderItem, object>> OrderByDesc { get; set; }
    }
    #endregion

    public class OrderItemListViewModel : GenericListViewModel<OrderItemModel>
    {
        public OrderItemListViewModel(IOrderItemService orderItemService, ICommonServices commonServices) : base(commonServices)
        {
            OrderItemService = orderItemService;
        }

        public IOrderItemService OrderItemService { get; }

        public OrderItemListArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(OrderItemListArgs args, bool silent = false)
        {
            ViewModelArgs = args ?? OrderItemListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            if (silent)
            {
                await RefreshAsync();
            }
            else
            {
                StartStatusMessage("Loading order items...");
                await RefreshAsync();
                EndStatusMessage("OrderItems loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderItemListViewModel>(this, OnMessage);
            MessageService.Subscribe<OrderItemDetailsViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public OrderItemListArgs CreateArgs()
        {
            return new OrderItemListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc,
                OrderID = ViewModelArgs.OrderID
            };
        }

        public async Task RefreshAsync()
        {
            Items = null;
            ItemsCount = 0;
            SelectedItem = null;

            Items = await GetItemsAsync();
            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                SelectedItem = Items.FirstOrDefault();
            }

            NotifyPropertyChanged(nameof(Title));
        }

        private async Task<IList<OrderItemModel>> GetItemsAsync()
        {
            if (ViewModelArgs.IsEmpty)
                return new List<OrderItemModel>();

            DataRequest<OrderItem> request = BuildDataRequest();
            return await OrderItemService.GetOrderItemsAsync(request);
        }

        protected override async void OnNew()
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<OrderItemDetailsViewModel>(new OrderItemDetailsArgs { OrderID = ViewModelArgs.OrderID });
            }
            else
            {
                NavigationService.Navigate<OrderItemDetailsViewModel>(new OrderItemDetailsArgs { OrderID = ViewModelArgs.OrderID });
            }

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading order items...");
            await RefreshAsync();
            EndStatusMessage("Order items loaded");
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected order items?", "Ok", "Cancel"))
            {
                if (SelectedIndexRanges != null)
                {
                    await DeleteRangesAsync(SelectedIndexRanges);
                    await RefreshAsync();
                    MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    SelectedIndexRanges = null;
                }
                else if (SelectedItems != null)
                {
                    await DeleteItemsAsync(SelectedItems);
                    await RefreshAsync();
                    MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    SelectedItems = null;
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<OrderItemModel> models)
        {
            foreach (var model in models)
            {
                await OrderItemService.DeleteOrderItemAsync(model);
            }
        }

        private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        {
            DataRequest<OrderItem> request = BuildDataRequest();
            foreach (var range in ranges)
            {
                await OrderItemService.DeleteOrderItemRangeAsync(range.Index, range.Length, request);
            }
        }

        private DataRequest<OrderItem> BuildDataRequest()
        {
            var request = new DataRequest<OrderItem>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
            if (ViewModelArgs.OrderID > 0)
            {
                request.Where = (r) => r.OrderID == ViewModelArgs.OrderID;
            }
            return request;
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
