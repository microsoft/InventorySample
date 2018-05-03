#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Services;
using Inventory.Models;

namespace Inventory.Services
{
    public class ProductService : IProductService
    {
        public ProductService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public async Task<ProductModel> GetProductAsync(string id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetProductAsync(dataService, id);
            }
        }
        static private async Task<ProductModel> GetProductAsync(IDataService dataService, string id)
        {
            var item = await dataService.GetProductAsync(id);
            if (item != null)
            {
                return await CreateProductModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<ProductModel>> GetProductsAsync(DataRequest<Product> request)
        {
            var collection = new ProductCollection(this, LogService);
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
                    model.Merge(await GetProductAsync(dataService, product.ProductID));
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
                Name = source.Name,
                Description = source.Description,
                Size = source.Size,
                Color = source.Color,
                ListPrice = source.ListPrice,
                DealerPrice = source.DealerPrice,
                TaxType = source.TaxType,
                Discount = source.Discount,
                DiscountStartDate = source.DiscountStartDate,
                DiscountEndDate = source.DiscountEndDate,
                StockUnits = source.StockUnits,
                SafetyStockLevel = source.SafetyStockLevel,
                CreatedOn = source.CreatedOn,
                LastModifiedOn = source.LastModifiedOn,
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
            target.Name = source.Name;
            target.Description = source.Description;
            target.Size = source.Size;
            target.Color = source.Color;
            target.ListPrice = source.ListPrice;
            target.DealerPrice = source.DealerPrice;
            target.TaxType = source.TaxType;
            target.Discount = source.Discount;
            target.DiscountStartDate = source.DiscountStartDate;
            target.DiscountEndDate = source.DiscountEndDate;
            target.StockUnits = source.StockUnits;
            target.SafetyStockLevel = source.SafetyStockLevel;
            target.CreatedOn = source.CreatedOn;
            target.LastModifiedOn = source.LastModifiedOn;
            target.Picture = source.Picture;
            target.Thumbnail = source.Thumbnail;
        }
    }
}
