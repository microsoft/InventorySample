using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    public class ProductListViewModel : ListViewModel<ProductModel>
    {
        public ProductListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
        }

        public ProductsViewState ViewState { get; private set; }

        public ICommand ItemInvokedCommand => new RelayCommand<ProductModel>(ItemInvoked);
        private async void ItemInvoked(ProductModel model)
        {
            await NavigationService.CreateNewViewAsync<ProductDetailsViewModel>(new ProductViewState { ProductID = model.ProductID });
        }

        public async Task LoadAsync(ProductsViewState state)
        {
            ViewState = state ?? ProductsViewState.CreateDefault();
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
                await NavigationService.CreateNewViewAsync<ProductDetailsViewModel>(new ProductViewState());
            }
            else
            {
                NavigationService.Navigate<ProductDetailsViewModel>(new ProductViewState());
            }
        }

        override public async Task<IList<ProductModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new DataRequest<Product>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            var virtualCollection = new ProductCollection(ProviderFactory.CreateDataProvider());
            await virtualCollection.RefreshAsync(request);
            return virtualCollection;
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<ProductModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteProductAsync(model);
            }
        }

        protected override async Task DeleteRangesAsync(IDataProvider dataProvider, IEnumerable<IndexRange> ranges)
        {
            var request = new DataRequest<Product>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            foreach (var range in ranges)
            {
                await dataProvider.DeleteProductRangeAsync(range.Index, range.Length, request);
            }
        }

        protected override async Task<bool> ConfirmDeleteSelectionAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected products?", "Ok", "Cancel");
        }

        public ProductsViewState GetCurrentState()
        {
            var state = ProductsViewState.CreateDefault();
            UpdateViewState(state);
            return state;
        }
    }
}
