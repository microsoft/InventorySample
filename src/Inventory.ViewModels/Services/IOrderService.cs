using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public interface IOrderService
    {
        Task<OrderModel> GetOrderAsync(long id);
        Task<IList<OrderModel>> GetOrdersAsync(DataRequest<Order> request);
        Task<IList<OrderModel>> GetOrdersAsync(int skip, int take, DataRequest<Order> request);
        Task<int> GetOrdersCountAsync(DataRequest<Order> request);

        Task<int> UpdateOrderAsync(OrderModel model);

        Task<int> DeleteOrderAsync(OrderModel model);
        Task<int> DeleteOrderRangeAsync(int index, int length, DataRequest<Order> request);
    }
}
