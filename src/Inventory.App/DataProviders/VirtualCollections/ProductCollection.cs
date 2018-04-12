using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Providers
{
    public class ProductCollection : VirtualCollection<ProductModel>
    {
        private DataRequest<Product> _dataRequest = null;

        public ProductCollection(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public IDataProvider DataProvider { get; private set; }

        private ProductModel _defaultItem = ProductModel.CreateEmpty();
        protected override ProductModel DefaultItem => _defaultItem;

        public async Task RefreshAsync(DataRequest<Product> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await DataProvider.GetProductsCountAsync(_dataRequest);
            Ranges[0] = await FetchDataAsync(0, RangeSize);
        }

        protected override async Task<IList<ProductModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            return await DataProvider.GetProductsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
        }

        #region Dispose
        public override void Dispose()
        {
            var dataProvider = DataProvider;
            DataProvider = null;
            if (dataProvider != null)
            {
                dataProvider.Dispose();
            }
        }
        #endregion
    }
}
