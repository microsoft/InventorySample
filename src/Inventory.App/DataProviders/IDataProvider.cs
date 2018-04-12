using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Providers
{
    public interface IDataProvider : IDisposable
    {
        Task<IList<CategoryModel>> GetCategoriesAsync();
        Task<IList<CountryCodeModel>> GetCountryCodesAsync();
        Task<IList<OrderStatusModel>> GetOrderStatusAsync();
        Task<IList<PaymentTypeModel>> GetPaymentTypesAsync();
        Task<IList<ShipperModel>> GetShippersAsync();
        Task<IList<TaxTypeModel>> GetTaxTypesAsync();

        Task<int> GetCustomersCountAsync(DataRequest<Customer> request);
        Task<IList<CustomerModel>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request);
        Task<CustomerModel> GetCustomerAsync(long id);
        Task<int> UpdateCustomerAsync(CustomerModel model);
        Task<int> DeleteCustomerAsync(CustomerModel model);
        Task<int> DeleteCustomerRangeAsync(int index, int length, DataRequest<Customer> request);

        Task<int> GetOrdersCountAsync(DataRequest<Order> request);
        Task<IList<OrderModel>> GetOrdersAsync(int skip, int take, DataRequest<Order> request);
        Task<OrderModel> GetOrderAsync(long id);
        Task<OrderModel> CreateNewOrderAsync(long customerID);
        Task<int> UpdateOrderAsync(OrderModel model);
        Task<int> DeleteOrderAsync(OrderModel model);
        Task<int> DeleteOrderRangeAsync(int index, int length, DataRequest<Order> request);

        Task<int> GetOrderItemsCountAsync(DataRequest<OrderItem> request);
        Task<IList<OrderItemModel>> GetOrderItemsAsync(int skip, int take, DataRequest<OrderItem> request);
        Task<OrderItemModel> GetOrderItemAsync(long orderID, int orderLine);
        Task<int> UpdateOrderItemAsync(OrderItemModel model);
        Task<int> DeleteOrderItemAsync(OrderItemModel model);
        Task<int> DeleteOrderItemRangeAsync(int index, int length, DataRequest<OrderItem> request);

        Task<int> GetProductsCountAsync(DataRequest<Product> request);
        Task<IList<ProductModel>> GetProductsAsync(int skip, int take, DataRequest<Product> request);
        Task<ProductModel> GetProductAsync(string id);
        Task<int> UpdateProductAsync(ProductModel model);
        Task<int> DeleteProductAsync(ProductModel model);
        Task<int> DeleteProductRangeAsync(int index, int length, DataRequest<Product> request);
    }
}
