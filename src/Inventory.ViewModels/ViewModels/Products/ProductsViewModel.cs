using System;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        public ProductsViewModel(IProductService productService, IOrderService orderService, ICommonServices commonServices) : base(commonServices)
        {
            ProductService = productService;

            ProductList = new ProductListViewModel(ProductService, commonServices);
            ProductDetails = new ProductDetailsViewModel(ProductService, commonServices);
        }

        public IProductService ProductService { get; }

        public ProductListViewModel ProductList { get; set; }
        public ProductDetailsViewModel ProductDetails { get; set; }

        public async Task LoadAsync(ProductListArgs args)
        {
            await ProductList.LoadAsync(args);
        }
        public void Unload()
        {
            ProductDetails.CancelEdit();
            ProductList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<ProductListViewModel>(this, OnMessage);
            ProductList.Subscribe();
            ProductDetails.Subscribe();
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            ProductList.Unsubscribe();
            ProductDetails.Unsubscribe();
        }

        private async void OnMessage(ProductListViewModel viewModel, string message, object args)
        {
            if (viewModel == ProductList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (ProductDetails.IsEditMode)
            {
                StatusReady();
                ProductDetails.CancelEdit();
            }
            var selected = ProductList.SelectedItem;
            if (!ProductList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            ProductDetails.Item = selected;
        }

        private async Task PopulateDetails(ProductModel selected)
        {
            var model = await ProductService.GetProductAsync(selected.ProductID);
            selected.Merge(model);
        }
    }
}
