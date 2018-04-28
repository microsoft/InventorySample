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
