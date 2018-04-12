using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Providers
{
    public class CustomerCollection : VirtualCollection<CustomerModel>
    {
        private DataRequest<Customer> _dataRequest = null;

        public CustomerCollection(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public IDataProvider DataProvider { get; private set; }

        private CustomerModel _defaultItem = CustomerModel.CreateEmpty();
        protected override CustomerModel DefaultItem => _defaultItem;

        public async Task RefreshAsync(DataRequest<Customer> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await DataProvider.GetCustomersCountAsync(_dataRequest);
            Ranges[0] = await FetchDataAsync(0, RangeSize);
        }

        protected override async Task<IList<CustomerModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            return await DataProvider.GetCustomersAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
        }

        #region Dispose
        public override void Dispose()
        {
            var dataProvider = DataProvider;
            DataProvider = null;
            if (dataProvider != null)
            {
                dataProvider.Dispose();
            }
        }
        #endregion
    }
}
