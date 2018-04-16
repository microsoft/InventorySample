using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class ProductCollection : VirtualCollection<ProductModel>
    {
        private DataRequest<Product> _dataRequest = null;

        public ProductCollection(IProductService productService)
        {
            ProductService = productService;
        }

        public IProductService ProductService { get; }

        private ProductModel _defaultItem = ProductModel.CreateEmpty();
        protected override ProductModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Product> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await ProductService.GetProductsCountAsync(_dataRequest);
            Ranges[0] = await FetchDataAsync(0, RangeSize);
        }

        protected override async Task<IList<ProductModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            return await ProductService.GetProductsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
        }
    }
}
