using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        public ProductsViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
        {
            ProviderFactory = providerFactory;

            ProductList = new ProductListViewModel(ProviderFactory, serviceManager);
            ProductList.PropertyChanged += OnListPropertyChanged;

            ProductDetails = new ProductDetailsViewModel(ProviderFactory, serviceManager);
            ProductDetails.ItemDeleted += OnItemDeleted;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public ProductListViewModel ProductList { get; set; }
        public ProductDetailsViewModel ProductDetails { get; set; }

        public async Task LoadAsync(ProductsViewState state)
        {
            await ProductList.LoadAsync(state);
        }

        public void Unload()
        {
            ProductList.Unload();
        }

        public async Task RefreshAsync()
        {
            await ProductList.RefreshAsync();
        }

        private async void OnListPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ProductListViewModel.SelectedItem):
                    ProductDetails.CancelEdit();
                    var selected = ProductList.SelectedItem;
                    if (!ProductList.IsMultipleSelection)
                    {
                        if (selected != null && !selected.IsEmpty)
                        {
                            await PopulateDetails(selected);
                        }
                    }
                    ProductDetails.Item = selected;
                    break;
                default:
                    break;
            }
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await ProductList.RefreshAsync();
        }

        private async Task PopulateDetails(ProductModel selected)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                var model = await dataProvider.GetProductAsync(selected.ProductID);
                selected.Merge(model);
            }
        }

        public void CancelEdit()
        {
            ProductDetails.CancelEdit();
        }
    }
}
