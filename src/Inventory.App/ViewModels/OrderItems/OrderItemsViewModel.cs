using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    public class OrderItemsViewModel : ViewModelBase
    {
        public OrderItemsViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
        {
            ProviderFactory = providerFactory;

            OrderItemList = new OrderItemListViewModel(ProviderFactory, serviceManager);
            OrderItemList.PropertyChanged += OnListPropertyChanged;

            OrderItemDetails = new OrderItemDetailsViewModel(ProviderFactory, serviceManager);
            OrderItemDetails.ItemDeleted += OnItemDeleted;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public OrderItemListViewModel OrderItemList { get; set; }
        public OrderItemDetailsViewModel OrderItemDetails { get; set; }

        public async Task LoadAsync(OrderItemsViewState state)
        {
            await OrderItemList.LoadAsync(state);
        }

        public void Unload()
        {
            OrderItemList.Unload();
        }

        public async Task RefreshAsync()
        {
            await OrderItemList.RefreshAsync();
        }

        private async void OnListPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OrderItemListViewModel.SelectedItem):
                    OrderItemDetails.CancelEdit();
                    if (!OrderItemList.IsMultipleSelection)
                    {
                        await PopulateDetails(OrderItemList.SelectedItem);
                    }
                    break;
                default:
                    break;
            }
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await OrderItemList.RefreshAsync();
        }

        private async Task PopulateDetails(OrderItemModel selected)
        {
            if (selected != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    var model = await dataProvider.GetOrderItemAsync(selected.OrderID, selected.OrderLine);
                    selected.Merge(model);
                }
            }
            OrderItemDetails.Item = selected;
        }

        public void CancelEdit()
        {
            OrderItemDetails.CancelEdit();
        }
    }
}
