using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    public class OrderItemListViewModel : ListViewModel<OrderItemModel>
    {
        public OrderItemListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
        }

        public OrderItemsViewState ViewState { get; private set; }

        public async Task LoadAsync(OrderItemsViewState state)
        {
            ViewState = state ?? OrderItemsViewState.CreateDefault();
            ApplyViewState(ViewState);
            await RefreshAsync();
        }

        public void Unload()
        {
            UpdateViewState(ViewState);
        }

        public override async void New()
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<OrderItemDetailsViewModel>(new OrderItemViewState(ViewState.OrderID));
            }
            else
            {
                NavigationService.Navigate<OrderItemDetailsViewModel>(new OrderItemViewState(ViewState.OrderID));
            }
        }

        override public async Task<IList<OrderItemModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new DataRequest<OrderItem>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            if (ViewState.OrderID > 0)
            {
                request.Where = (r) => r.OrderID == ViewState.OrderID;
            }
            return await dataProvider.GetOrderItemsAsync(0, -1, request);
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<OrderItemModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteOrderItemAsync(model);
            }
        }

        protected override Task DeleteRangesAsync(IDataProvider dataProvider, IEnumerable<IndexRange> ranges)
        {
            throw new NotImplementedException();
        }

        protected override async Task<bool> ConfirmDeleteSelectionAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected order items?", "Ok", "Cancel");
        }

        public CustomersViewState GetCurrentState()
        {
            var state = CustomersViewState.CreateDefault();
            UpdateViewState(state);
            return state;
        }
    }
}
