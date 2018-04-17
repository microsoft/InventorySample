using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region ProductDetailsArgs
    public class ProductDetailsArgs
    {
        static public ProductDetailsArgs CreateDefault() => new ProductDetailsArgs();

        public string ProductID { get; set; }

        public bool IsNew => String.IsNullOrEmpty(ProductID);
    }
    #endregion

    public class ProductDetailsViewModel : GenericDetailsViewModel<ProductModel>
    {
        public ProductDetailsViewModel(IProductService productService, ICommonServices commonServices) : base(commonServices)
        {
            ProductService = productService;
        }

        public IProductService ProductService { get; }

        override public string Title => (Item?.IsNew ?? true) ? "New Product" : TitleEdit;
        public string TitleEdit => Item == null ? "Product" : $"{Item.Name}";

        public ProductDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(ProductDetailsArgs args)
        {
            ViewModelArgs = args ?? ProductDetailsArgs.CreateDefault();

            if (ViewModelArgs.IsNew)
            {
                Item = new ProductModel();
                IsEditMode = true;
            }
            else
            {
                var item = await ProductService.GetProductAsync(ViewModelArgs.ProductID);
                Item = item ?? new ProductModel { ProductID = ViewModelArgs.ProductID, IsEmpty = true };
            }
        }
        public void Unload()
        {
            ViewModelArgs.ProductID = Item?.ProductID;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<ProductDetailsViewModel, ProductModel>(this, OnDetailsMessage);
            MessageService.Subscribe<ProductListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public ProductDetailsArgs CreateArgs()
        {
            return new ProductDetailsArgs
            {
                ProductID = Item?.ProductID
            };
        }

        protected override async Task SaveItemAsync(ProductModel model)
        {
            StartStatusMessage("Saving product...");
            await Task.Delay(100);
            await ProductService.UpdateProductAsync(model);
            EndStatusMessage("Product saved");
        }

        protected override async Task DeleteItemAsync(ProductModel model)
        {
            StartStatusMessage("Deleting product...");
            await Task.Delay(100);
            await ProductService.DeleteProductAsync(model);
            EndStatusMessage("Product deleted");
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current product?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<ProductModel>> GetValidationConstraints(ProductModel model)
        {
            yield return new RequiredConstraint<ProductModel>("Name", m => m.Name);
            yield return new RequiredGreaterThanZeroConstraint<ProductModel>("Category", m => m.CategoryID);
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(ProductDetailsViewModel sender, string message, ProductModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.ProductID == current?.ProductID)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                var item = await ProductService.GetProductAsync(current.ProductID);
                                item = item ?? new ProductModel { ProductID = current.ProductID, IsEmpty = true };
                                current.Merge(item);
                                current.NotifyChanges();
                                NotifyPropertyChanged(nameof(Title));
                                if (IsEditMode)
                                {
                                    StatusMessage("WARNING: This product has been modified externally");
                                }
                            });
                            break;
                        case "ItemDeleted":
                            await OnItemDeletedExternally();
                            break;
                    }
                }
            }
        }

        private async void OnListMessage(ProductListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<ProductModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.ProductID == current.ProductID))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        var model = await ProductService.GetProductAsync(current.ProductID);
                        if (model == null)
                        {
                            await OnItemDeletedExternally();
                        }
                        break;
                }
            }
        }

        private async Task OnItemDeletedExternally()
        {
            await ContextService.RunAsync(() =>
            {
                CancelEdit();
                IsEnabled = false;
                StatusMessage("WARNING: This product has been deleted externally");
            });
        }
    }
}
