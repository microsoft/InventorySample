using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    public class ProductDetailsViewModel : DetailsViewModel<ProductModel>
    {
        public ProductDetailsViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
        }

        override public string Title => (Item?.IsNew ?? true) ? TitleNew : TitleEdit;
        public string TitleNew => "New Product";
        public string TitleEdit => Item == null ? "Product" : $"{Item.Name}";

        public override bool IsNewItem => Item?.IsNew ?? false;

        protected override void ItemUpdated()
        {
            NotifyPropertyChanged(nameof(Title));
        }

        public async Task LoadAsync(ProductViewState state)
        {
            if (!state.IsNew)
            {
                using (var dp = ProviderFactory.CreateDataProvider())
                {
                    var item = await dp.GetProductAsync(state.ProductID);
                    Item = item ?? new ProductModel { ProductID = state.ProductID, IsDeleted = true };
                }
            }
            else
            {
                Item = new ProductModel();
                IsEditMode = true;
            }
        }

        protected override async Task SaveItemAsync(ProductModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await Task.Delay(100);
                await dataProvider.UpdateProductAsync(model);
                NotifyPropertyChanged(nameof(Title));
            }
        }

        protected override async Task DeleteItemAsync(ProductModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await dataProvider.DeleteProductAsync(model);
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current product?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<ProductModel>> ValidationConstraints
        {
            get
            {
                // TODOX: 
                yield break;
            }
        }
    }
}
