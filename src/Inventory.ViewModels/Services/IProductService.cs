using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public interface IProductService
    {
        Task<ProductModel> GetProductAsync(string id);
        Task<IList<ProductModel>> GetProductsAsync(DataRequest<Product> request);
        Task<IList<ProductModel>> GetProductsAsync(int skip, int take, DataRequest<Product> request);
        Task<int> GetProductsCountAsync(DataRequest<Product> request);

        Task<int> UpdateProductAsync(ProductModel model);

        Task<int> DeleteProductAsync(ProductModel model);
        Task<int> DeleteProductRangeAsync(int index, int length, DataRequest<Product> request);
    }
}
