using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data.Services;
using Inventory.Models;

namespace Inventory.Providers
{
    public partial class SQLBaseProvider : IDataProvider
    {
        public SQLBaseProvider(DataServiceBase dataService)
        {
            DataService = dataService;
        }

        public DataServiceBase DataService { get; }

        public async Task<IList<CategoryModel>> GetCategoriesAsync()
        {
            var items = await DataService.GetCategoriesAsync();
            return items.Select(r => new CategoryModel
            {
                CategoryID = r.CategoryID,
                Name = r.Name
            })
            .ToList();
        }

        public async Task<IList<CountryCodeModel>> GetCountryCodesAsync()
        {
            var items = await DataService.GetCountryCodesAsync();
            return items.OrderBy(r => r.Name).Select(r => new CountryCodeModel
            {
                CountryCodeID = r.CountryCodeID,
                Name = r.Name
            })
            .ToList();
        }

        public async Task<IList<OrderStatusModel>> GetOrderStatusAsync()
        {
            var items = await DataService.GetOrderStatusAsync();
            return items.Select(r => new OrderStatusModel
            {
                Status = r.Status,
                Name = r.Name
            })
            .ToList();
        }

        public async Task<IList<PaymentTypeModel>> GetPaymentTypesAsync()
        {
            var items = await DataService.GetPaymentTypesAsync();
            return items.Select(r => new PaymentTypeModel
            {
                PaymentTypeID = r.PaymentTypeID,
                Name = r.Name
            })
            .ToList();
        }

        public async Task<IList<ShipperModel>> GetShippersAsync()
        {
            var items = await DataService.GetShippersAsync();
            return items.Select(r => new ShipperModel
            {
                ShipperID = r.ShipperID,
                Name = r.Name,
                Phone = r.Phone
            })
            .ToList();
        }

        public async Task<IList<TaxTypeModel>> GetTaxTypesAsync()
        {
            var items = await DataService.GetTaxTypesAsync();
            return items.Select(r => new TaxTypeModel
            {
                TaxTypeID = r.TaxTypeID,
                Name = r.Name,
                Rate = r.Rate
            })
            .ToList();
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DataService != null)
                {
                    DataService.Dispose();
                }
            }
        }
        #endregion
    }
}
