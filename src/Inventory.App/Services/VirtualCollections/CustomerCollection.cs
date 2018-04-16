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

        public CustomerCollection(ICustomerService customerService)
        {
            CustomerService = customerService;
        }

        public ICustomerService CustomerService { get; }

        private CustomerModel _defaultItem = CustomerModel.CreateEmpty();
        protected override CustomerModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Customer> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await CustomerService.GetCustomersCountAsync(_dataRequest);
            Ranges[0] = await FetchDataAsync(0, RangeSize);
        }

        protected override async Task<IList<CustomerModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            return await CustomerService.GetCustomersAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
        }
    }
}
