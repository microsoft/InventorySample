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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Data.Services
{
    public interface IDataService : IDisposable
    {
        Task<Customer> GetCustomerAsync(long id);
        Task<IList<Customer>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request);
        Task<IList<Customer>> GetCustomerKeysAsync(int skip, int take, DataRequest<Customer> request);
        Task<int> GetCustomersCountAsync(DataRequest<Customer> request);
        Task<int> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomersAsync(params Customer[] customers);

        Task<Order> GetOrderAsync(long id);
        Task<IList<Order>> GetOrdersAsync(int skip, int take, DataRequest<Order> request);
        Task<IList<Order>> GetOrderKeysAsync(int skip, int take, DataRequest<Order> request);
        Task<int> GetOrdersCountAsync(DataRequest<Order> request);
        Task<int> UpdateOrderAsync(Order order);
        Task<int> DeleteOrdersAsync(params Order[] orders);

        Task<OrderItem> GetOrderItemAsync(long orderID, int orderLine);
        Task<IList<OrderItem>> GetOrderItemsAsync(int skip, int take, DataRequest<OrderItem> request);
        Task<IList<OrderItem>> GetOrderItemKeysAsync(int skip, int take, DataRequest<OrderItem> request);
        Task<int> GetOrderItemsCountAsync(DataRequest<OrderItem> request);
        Task<int> UpdateOrderItemAsync(OrderItem orderItem);
        Task<int> DeleteOrderItemsAsync(params OrderItem[] orderItems);

        Task<Product> GetProductAsync(string id);
        Task<IList<Product>> GetProductsAsync(int skip, int take, DataRequest<Product> request);
        Task<IList<Product>> GetProductKeysAsync(int skip, int take, DataRequest<Product> request);
        Task<int> GetProductsCountAsync(DataRequest<Product> request);
        Task<int> UpdateProductAsync(Product product);
        Task<int> DeleteProductsAsync(params Product[] products);


        Task<IList<Category>> GetCategoriesAsync();
        Task<IList<CountryCode>> GetCountryCodesAsync();
        Task<IList<OrderStatus>> GetOrderStatusAsync();
        Task<IList<PaymentType>> GetPaymentTypesAsync();
        Task<IList<Shipper>> GetShippersAsync();
        Task<IList<TaxType>> GetTaxTypesAsync();
    }
}
