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
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class LookupTables : ILookupTables
    {
        public LookupTables(ILogService logService, IDataServiceFactory dataServiceFactory)
        {
            LogService = logService;
            DataServiceFactory = dataServiceFactory;
        }

        public ILogService LogService { get; }
        public IDataServiceFactory DataServiceFactory { get; }

        public IList<CategoryModel> Categories { get; private set; }
        public IList<CountryCodeModel> CountryCodes { get; private set; }
        public IList<OrderStatusModel> OrderStatus { get; private set; }
        public IList<PaymentTypeModel> PaymentTypes { get; private set; }
        public IList<ShipperModel> Shippers { get; private set; }
        public IList<TaxTypeModel> TaxTypes { get; private set; }

        public async Task InitializeAsync()
        {
            Categories = await GetCategoriesAsync();
            CountryCodes = await GetCountryCodesAsync();
            OrderStatus = await GetOrderStatusAsync();
            PaymentTypes = await GetPaymentTypesAsync();
            Shippers = await GetShippersAsync();
            TaxTypes = await GetTaxTypesAsync();
        }

        public string GetCategory(int id)
        {
            return Categories.Where(r => r.CategoryID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetCountry(string id)
        {
            return CountryCodes.Where(r => r.CountryCodeID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetOrderStatus(int id)
        {
            return OrderStatus.Where(r => r.Status == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetPaymentType(int? id)
        {
            return id == null ? "" : PaymentTypes.Where(r => r.PaymentTypeID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetShipper(int? id)
        {
            return id == null ? "" : Shippers.Where(r => r.ShipperID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetTaxDesc(int id)
        {
            return TaxTypes.Where(r => r.TaxTypeID == id).Select(r => $"{r.Rate} %").FirstOrDefault();
        }
        public decimal GetTaxRate(int id)
        {
            return TaxTypes.Where(r => r.TaxTypeID == id).Select(r => r.Rate).FirstOrDefault();
        }

        private async Task<IList<CategoryModel>> GetCategoriesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetCategoriesAsync();
                    return items.Select(r => new CategoryModel
                    {
                        CategoryID = r.CategoryID,
                        Name = r.Name
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load Categories", ex);
            }
            return new List<CategoryModel>();
        }

        private async Task<IList<CountryCodeModel>> GetCountryCodesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetCountryCodesAsync();
                    return items.OrderBy(r => r.Name).Select(r => new CountryCodeModel
                    {
                        CountryCodeID = r.CountryCodeID,
                        Name = r.Name
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load CountryCodes", ex);
            }
            return new List<CountryCodeModel>();
        }

        private async Task<IList<OrderStatusModel>> GetOrderStatusAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetOrderStatusAsync();
                    return items.Select(r => new OrderStatusModel
                    {
                        Status = r.Status,
                        Name = r.Name
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load OrderStatus", ex);
            }
            return new List<OrderStatusModel>();
        }

        private async Task<IList<PaymentTypeModel>> GetPaymentTypesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetPaymentTypesAsync();
                    return items.Select(r => new PaymentTypeModel
                    {
                        PaymentTypeID = r.PaymentTypeID,
                        Name = r.Name
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load PaymentTypes", ex);
            }
            return new List<PaymentTypeModel>();
        }

        private async Task<IList<ShipperModel>> GetShippersAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetShippersAsync();
                    return items.Select(r => new ShipperModel
                    {
                        ShipperID = r.ShipperID,
                        Name = r.Name,
                        Phone = r.Phone
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load Shippers", ex);
            }
            return new List<ShipperModel>();
        }

        private async Task<IList<TaxTypeModel>> GetTaxTypesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetTaxTypesAsync();
                    return items.Select(r => new TaxTypeModel
                    {
                        TaxTypeID = r.TaxTypeID,
                        Name = r.Name,
                        Rate = r.Rate
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load TaxTypes", ex);
            }
            return new List<TaxTypeModel>();
        }

        private async void LogException(string source, string action, Exception exception)
        {
            await LogService.WriteAsync(LogType.Error, source, action, exception.Message, exception.ToString());
        }
    }
}
