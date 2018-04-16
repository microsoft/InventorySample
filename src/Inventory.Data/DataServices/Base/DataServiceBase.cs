using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Services
{
    abstract public partial class DataServiceBase : IDataService, IDisposable
    {
        private IDataSource _dataSource = null;

        public DataServiceBase(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<IList<Category>> GetCategoriesAsync()
        {
            return await _dataSource.Categories.ToListAsync();
        }

        public async Task<IList<CountryCode>> GetCountryCodesAsync()
        {
            return await _dataSource.CountryCodes.ToListAsync();
        }

        public async Task<IList<OrderStatus>> GetOrderStatusAsync()
        {
            return await _dataSource.OrderStatus.ToListAsync();
        }

        public async Task<IList<PaymentType>> GetPaymentTypesAsync()
        {
            return await _dataSource.PaymentTypes.ToListAsync();
        }

        public async Task<IList<Shipper>> GetShippersAsync()
        {
            return await _dataSource.Shippers.ToListAsync();
        }

        public async Task<IList<TaxType>> GetTaxTypesAsync()
        {
            return await _dataSource.TaxTypes.ToListAsync();
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
                if (_dataSource != null)
                {
                    _dataSource.Dispose();
                }
            }
        }
        #endregion
    }
}
