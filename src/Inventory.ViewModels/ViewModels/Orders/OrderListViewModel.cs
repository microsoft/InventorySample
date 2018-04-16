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
    #region OrderListArgs
    public class OrderListArgs
    {
        static public OrderListArgs CreateEmpty() => new OrderListArgs { IsEmpty = true };

        public OrderListArgs()
        {
            OrderByDesc = r => r.OrderDate;
        }

        public bool IsEmpty { get; set; }

        public long CustomerID { get; set; }

        public string Query { get; set; }

        public Expression<Func<Order, object>> OrderBy { get; set; }
        public Expression<Func<Order, object>> OrderByDesc { get; set; }
    }
    #endregion

    public class OrderListViewModel : GenericListViewModel<OrderModel>
    {
        public OrderListViewModel(IOrderService orderService, ICommonServices commonServices) : base(commonServices)
        {
            OrderService = orderService;
        }

        public IOrderService OrderService { get; }

        public OrderListArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(OrderListArgs args, bool silent = false)
        {
            ViewModelArgs = args ?? OrderListArgs.CreateEmpty();
            Query = args.Query;

            if (silent)
            {
                await RefreshAsync();
            }
            else
            {
                StartStatusMessage("Loading orders...");
                await RefreshAsync();
                EndStatusMessage("Orders loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderListViewModel>(this, OnMessage);
            MessageService.Subscribe<OrderDetailsViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public OrderListArgs CreateArgs()
        {
            return new OrderListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc,
                CustomerID = ViewModelArgs.CustomerID
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

        private async Task<IList<OrderModel>> GetItemsAsync()
        {
            if (ViewModelArgs.IsEmpty)
                return new List<OrderModel>();

            DataRequest<Order> request = BuildDataRequest();
            return await OrderService.GetOrdersAsync(request);
        }

        protected override async void OnNew()
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<OrderDetailsViewModel>(new OrderDetailsArgs { CustomerID = ViewModelArgs.CustomerID });
            }
            else
            {
                NavigationService.Navigate<OrderDetailsViewModel>(new OrderDetailsArgs { CustomerID = ViewModelArgs.CustomerID });
            }

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading orders...");
            await RefreshAsync();
            EndStatusMessage("Orders loaded");
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected orders?", "Ok", "Cancel"))
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

        private async Task DeleteItemsAsync(IEnumerable<OrderModel> models)
        {
            foreach (var model in models)
            {
                await OrderService.DeleteOrderAsync(model);
            }
        }

        private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        {
            DataRequest<Order> request = BuildDataRequest();
            foreach (var range in ranges)
            {
                await OrderService.DeleteOrderRangeAsync(range.Index, range.Length, request);
            }
        }

        private DataRequest<Order> BuildDataRequest()
        {
            var request = new DataRequest<Order>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
            if (ViewModelArgs.CustomerID > 0)
            {
                request.Where = (r) => r.CustomerID == ViewModelArgs.CustomerID;
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
