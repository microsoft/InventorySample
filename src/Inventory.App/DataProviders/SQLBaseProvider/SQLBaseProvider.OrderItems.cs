using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Providers
{
    partial class SQLBaseProvider
    {
        public async Task<int> GetOrderItemsCountAsync(DataRequest<OrderItem> request)
        {
            return await DataService.GetOrderItemsCountAsync(request);
        }

        public async Task<IList<OrderItemModel>> GetOrderItemsAsync(int skip, int take, DataRequest<OrderItem> request)
        {
            var models = new List<OrderItemModel>();
            var items = await DataService.GetOrderItemsAsync(skip, take, request);
            foreach (var item in items)
            {
                models.Add(await CreateOrderItemModelAsync(item, includeAllFields: false));
            }
            return models;
        }

        public async Task<OrderItemModel> GetOrderItemAsync(long orderID, int lineID)
        {
            var item = await DataService.GetOrderItemAsync(orderID, lineID);
            if (item != null)
            {
                return await CreateOrderItemModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<int> UpdateOrderItemAsync(OrderItemModel model)
        {
            var orderItem = model.OrderLine > 0 ? await DataService.GetOrderItemAsync(model.OrderID, model.OrderLine) : new OrderItem();
            if (orderItem != null)
            {
                UpdateOrderItemFromModel(orderItem, model);
                await DataService.UpdateOrderItemAsync(orderItem);
                model.Merge(await GetOrderItemAsync(orderItem.OrderID, orderItem.OrderLine));
            }
            return 0;
        }

        public async Task<int> DeleteOrderItemAsync(OrderItemModel model)
        {
            var orderItem = new OrderItem { OrderID = model.OrderID, OrderLine = model.OrderLine };
            return await DataService.DeleteOrderItemsAsync(orderItem);
        }

        public async Task<int> DeleteOrderItemRangeAsync(int index, int length, DataRequest<OrderItem> request)
        {
            var items = await DataService.GetOrderItemKeysAsync(index, length, request);
            return await DataService.DeleteOrderItemsAsync(items.ToArray());
        }

        private async Task<OrderItemModel> CreateOrderItemModelAsync(OrderItem source, bool includeAllFields)
        {
            var model = new OrderItemModel()
            {
                OrderID = source.OrderID,
                OrderLine = source.OrderLine,
                ProductID = source.ProductID,
                Quantity = source.Quantity,
                UnitPrice = source.UnitPrice,
                Discount = source.Discount,
                TaxType = source.TaxType,
                Product = await CreateProductModelAsync(source.Product, includeAllFields)
            };
            return model;
        }

        private void UpdateOrderItemFromModel(OrderItem target, OrderItemModel source)
        {
            target.OrderID = source.OrderID;
            target.OrderLine = source.OrderLine;
            target.ProductID = source.ProductID;
            target.Quantity = source.Quantity;
            target.UnitPrice = source.UnitPrice;
            target.Discount = source.Discount;
            target.TaxType = source.TaxType;
        }
    }
}
