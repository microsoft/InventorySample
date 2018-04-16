using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region ProductListArgs
    public class ProductListArgs
    {
        static public ProductListArgs CreateEmpty() => new ProductListArgs { IsEmpty = true };

        public ProductListArgs()
        {
            OrderBy = r => r.Name;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Product, object>> OrderBy { get; set; }
        public Expression<Func<Product, object>> OrderByDesc { get; set; }
    }
    #endregion

    public class ProductListViewModel : GenericListViewModel<ProductModel>
    {
        public ProductListViewModel(IProductService productService, ICommonServices commonServices) : base(commonServices)
        {
            ProductService = productService;
        }

        public IProductService ProductService { get; }

        public ProductListArgs ViewModelArgs { get; private set; }

        public ICommand ItemInvokedCommand => new RelayCommand<ProductModel>(ItemInvoked);
        private async void ItemInvoked(ProductModel model)
        {
            await NavigationService.CreateNewViewAsync<ProductDetailsViewModel>(new ProductDetailsArgs { ProductID = model.ProductID });
        }

        public async Task LoadAsync(ProductListArgs args)
        {
            ViewModelArgs = args ?? ProductListArgs.CreateEmpty();
            Query = args.Query;

            StartStatusMessage("Loading products...");
            await RefreshAsync();
            EndStatusMessage("Products loaded");
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<ProductListViewModel>(this, OnMessage);
            MessageService.Subscribe<ProductDetailsViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public ProductListArgs CreateArgs()
        {
            return new ProductListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
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

        private async Task<IList<ProductModel>> GetItemsAsync()
        {
            if (ViewModelArgs.IsEmpty)
                return new List<ProductModel>();

            DataRequest<Product> request = BuildDataRequest();
            return await ProductService.GetProductsAsync(request);
        }

        protected override async void OnNew()
        {

            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<ProductDetailsViewModel>(new ProductDetailsArgs());
            }
            else
            {
                NavigationService.Navigate<ProductDetailsViewModel>(new ProductDetailsArgs());
            }

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading products...");
            await RefreshAsync();
            EndStatusMessage("Products loaded");
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected products?", "Ok", "Cancel"))
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

        private async Task DeleteItemsAsync(IEnumerable<ProductModel> models)
        {
            foreach (var model in models)
            {
                await ProductService.DeleteProductAsync(model);
            }
        }

        private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        {
            DataRequest<Product> request = BuildDataRequest();
            foreach (var range in ranges)
            {
                await ProductService.DeleteProductRangeAsync(range.Index, range.Length, request);
            }
        }

        private DataRequest<Product> BuildDataRequest()
        {
            return new DataRequest<Product>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
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
