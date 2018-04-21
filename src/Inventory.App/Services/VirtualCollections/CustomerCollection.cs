using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class CustomerCollection : VirtualCollection<CustomerModel>
    {
        private DataRequest<Customer> _dataRequest = null;

        public CustomerCollection(ICustomerService customerService, ILogService logService) : base(logService)
        {
            CustomerService = customerService;
        }

        public ICustomerService CustomerService { get; }

        private CustomerModel _defaultItem = CustomerModel.CreateEmpty();
        protected override CustomerModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Customer> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await CustomerService.GetCustomersCountAsync(_dataRequest);
                Ranges[0] = await CustomerService.GetCustomersAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<CustomerModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await CustomerService.GetCustomersAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("CustomerCollection", "Fetch", ex);
            }
            return null;
        }
    }
}
