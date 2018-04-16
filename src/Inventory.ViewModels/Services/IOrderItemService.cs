using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public interface IOrderItemService
    {
        Task<OrderItemModel> GetOrderItemAsync(long orderID, int lineID);
        Task<IList<OrderItemModel>> GetOrderItemsAsync(DataRequest<OrderItem> request);
        Task<IList<OrderItemModel>> GetOrderItemsAsync(int skip, int take, DataRequest<OrderItem> request);
        Task<int> GetOrderItemsCountAsync(DataRequest<OrderItem> request);

        Task<int> UpdateOrderItemAsync(OrderItemModel model);

        Task<int> DeleteOrderItemAsync(OrderItemModel model);
        Task<int> DeleteOrderItemRangeAsync(int index, int length, DataRequest<OrderItem> request);
    }
}
