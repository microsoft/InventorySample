using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Providers
{
    public class OrderCollection : VirtualCollection<OrderModel>
    {
        private DataRequest<Order> _dataRequest = null;

        public OrderCollection(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public IDataProvider DataProvider { get; private set; }

        private OrderModel _defaultItem = OrderModel.CreateEmpty();
        protected override OrderModel DefaultItem => null;

        public async Task RefreshAsync(DataRequest<Order> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await DataProvider.GetOrdersCountAsync(_dataRequest);
            Ranges[0] = await FetchDataAsync(0, RangeSize);
        }

        protected override async Task<IList<OrderModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            return await DataProvider.GetOrdersAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
        }

        public override void Dispose()
        {
            var dataProvider = DataProvider;
            DataProvider = null;
            if (dataProvider != null)
            {
                dataProvider.Dispose();
            }
        }
    }
}
