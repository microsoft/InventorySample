using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        public OrdersViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
        {
            ProviderFactory = providerFactory;

            OrderList = new OrderListViewModel(ProviderFactory, serviceManager);
            OrderList.PropertyChanged += OnListPropertyChanged;

            OrderDetails = new OrderDetailsViewModel(ProviderFactory, serviceManager);
            OrderDetails.ItemDeleted += OnItemDeleted;

            OrderItemList = new OrderItemListViewModel(ProviderFactory, serviceManager);
        }

        public IDataProviderFactory ProviderFactory { get; }

        public OrderListViewModel OrderList { get; set; }
        public OrderDetailsViewModel OrderDetails { get; set; }
        public OrderItemListViewModel OrderItemList { get; set; }

        public async Task LoadAsync(OrdersViewState state)
        {
            await OrderList.LoadAsync(state);
        }

        public void Unload()
        {
            OrderList.Unload();
        }

        public async Task RefreshAsync()
        {
            await OrderList.RefreshAsync();
        }

        private async void OnListPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OrderListViewModel.SelectedItem):
                    OrderDetails.CancelEdit();
                    OrderItemList.IsMultipleSelection = false;
                    var selected = OrderList.SelectedItem;
                    if (!OrderList.IsMultipleSelection)
                    {
                        if (selected != null && !selected.IsEmpty)
                        {
                            await PopulateDetails(selected);
                            await PopulateOrderItems(selected);
                        }
                    }
                    OrderDetails.Item = selected;
                    break;
                default:
                    break;
            }
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await OrderList.RefreshAsync();
        }

        private async Task PopulateDetails(OrderModel selected)
        {
            if (selected != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    var model = await dataProvider.GetOrderAsync(selected.OrderID);
                    selected.Merge(model);
                }
            }
        }

        private async Task PopulateOrderItems(OrderModel selected)
        {
            if (selected != null)
            {
                await OrderItemList.LoadAsync(new OrderItemsViewState { OrderID = selected.OrderID });
            }
        }

        public void CancelEdit()
        {
            OrderDetails.CancelEdit();
        }
    }
}
