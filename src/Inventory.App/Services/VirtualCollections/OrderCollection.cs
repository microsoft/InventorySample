using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class OrderCollection : VirtualCollection<OrderModel>
    {
        private DataRequest<Order> _dataRequest = null;

        public OrderCollection(IOrderService orderService)
        {
            OrderService = orderService;
        }

        public IOrderService OrderService { get; }

        private OrderModel _defaultItem = OrderModel.CreateEmpty();
        protected override OrderModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Order> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await OrderService.GetOrdersCountAsync(_dataRequest);
            Ranges[0] = await FetchDataAsync(0, RangeSize);
        }

        protected override async Task<IList<OrderModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            return await OrderService.GetOrdersAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
        }
    }
}
