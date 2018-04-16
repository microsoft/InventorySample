using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class ProductService : IProductService
    {
        public ProductService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public IDataServiceFactory DataServiceFactory { get; }

        public async Task<ProductModel> GetProductAsync(string id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var item = await dataService.GetProductAsync(id);
                if (item != null)
                {
                    return await CreateProductModelAsync(item, includeAllFields: true);
                }
                return null;
            }
        }

        public async Task<IList<ProductModel>> GetProductsAsync(DataRequest<Product> request)
        {
            var collection = new ProductCollection(this);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<ProductModel>> GetProductsAsync(int skip, int take, DataRequest<Product> request)
        {
            var models = new List<ProductModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetProductsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateProductModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetProductsCountAsync(DataRequest<Product> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetProductsCountAsync(request);
            }
        }

        public async Task<int> UpdateProductAsync(ProductModel model)
        {
            string id = model.ProductID;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var product = !String.IsNullOrEmpty(id) ? await dataService.GetProductAsync(model.ProductID) : new Product();
                if (product != null)
                {
                    UpdateProductFromModel(product, model);
                    await dataService.UpdateProductAsync(product);
                    model.Merge(await GetProductAsync(product.ProductID));
                }
                return 0;
            }
        }

        public async Task<int> DeleteProductAsync(ProductModel model)
        {
            var product = new Product { ProductID = model.ProductID };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteProductsAsync(product);
            }
        }

        public async Task<int> DeleteProductRangeAsync(int index, int length, DataRequest<Product> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetProductKeysAsync(index, length, request);
                return await dataService.DeleteProductsAsync(items.ToArray());
            }
        }

        static public async Task<ProductModel> CreateProductModelAsync(Product source, bool includeAllFields)
        {
            var model = new ProductModel()
            {
                ProductID = source.ProductID,
                CategoryID = source.CategoryID,
                SubCategoryID = source.SubCategoryID,
                Name = source.Name,
                Description = source.Description,
                Size = source.Size,
                Color = source.Color,
                Gender = source.Gender,
                ListPrice = source.ListPrice,
                DealerPrice = source.DealerPrice,
                TaxType = source.TaxType,
                Discount = source.Discount,
                DiscountStartDate = source.DiscountStartDate.AsNullableDateTimeOffset(),
                DiscountEndDate = source.DiscountEndDate.AsNullableDateTimeOffset(),
                StockUnits = source.StockUnits,
                SafetyStockLevel = source.SafetyStockLevel,
                StartDate = source.StartDate.AsNullableDateTimeOffset(),
                EndDate = source.EndDate.AsNullableDateTimeOffset(),
                CreatedOn = source.CreatedOn.AsDateTimeOffset(),
                LastModifiedOn = source.LastModifiedOn.AsDateTimeOffset(),
                Thumbnail = source.Thumbnail,
                ThumbnailSource = await BitmapTools.LoadBitmapAsync(source.Thumbnail)
            };

            if (includeAllFields)
            {
                model.Picture = source.Picture;
                model.PictureSource = await BitmapTools.LoadBitmapAsync(source.Picture);
            }
            return model;
        }

        private void UpdateProductFromModel(Product target, ProductModel source)
        {
            target.CategoryID = source.CategoryID;
            target.SubCategoryID = source.SubCategoryID;
            target.Name = source.Name;
            target.Description = source.Description;
            target.Size = source.Size;
            target.Color = source.Color;
            target.Gender = source.Gender;
            target.ListPrice = source.ListPrice;
            target.DealerPrice = source.DealerPrice;
            target.TaxType = source.TaxType;
            target.Discount = source.Discount;
            target.DiscountStartDate = source.DiscountStartDate.AsNullableDateTime();
            target.DiscountEndDate = source.DiscountEndDate.AsNullableDateTime();
            target.StockUnits = source.StockUnits;
            target.SafetyStockLevel = source.SafetyStockLevel;
            target.StartDate = source.StartDate.AsNullableDateTime();
            target.EndDate = source.EndDate.AsNullableDateTime();
            target.CreatedOn = source.CreatedOn.AsDateTime();
            target.LastModifiedOn = source.LastModifiedOn.AsDateTime();
            target.Picture = source.Picture;
            target.Thumbnail = source.Thumbnail;
        }
    }
}
