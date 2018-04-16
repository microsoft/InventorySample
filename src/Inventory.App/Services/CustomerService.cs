using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class CustomerService : ICustomerService
    {
        public CustomerService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public IDataServiceFactory DataServiceFactory { get; }

        public async Task<CustomerModel> GetCustomerAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var item = await dataService.GetCustomerAsync(id);
                if (item != null)
                {
                    return await CreateCustomerModelAsync(item, includeAllFields: true);
                }
                return null;
            }
        }

        public async Task<IList<CustomerModel>> GetCustomersAsync(DataRequest<Customer> request)
        {
            var collection = new CustomerCollection(this);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<CustomerModel>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request)
        {
            var models = new List<CustomerModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCustomersAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateCustomerModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetCustomersCountAsync(DataRequest<Customer> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetCustomersCountAsync(request);
            }
        }

        public async Task<int> UpdateCustomerAsync(CustomerModel model)
        {
            long id = model.CustomerID;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var customer = id > 0 ? await dataService.GetCustomerAsync(model.CustomerID) : new Customer();
                if (customer != null)
                {
                    UpdateCustomerFromModel(customer, model);
                    await dataService.UpdateCustomerAsync(customer);
                    model.Merge(await GetCustomerAsync(customer.CustomerID));
                }
                return 0;
            }
        }

        public async Task<int> DeleteCustomerAsync(CustomerModel model)
        {
            var customer = new Customer { CustomerID = model.CustomerID };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteCustomersAsync(customer);
            }
        }

        public async Task<int> DeleteCustomerRangeAsync(int index, int length, DataRequest<Customer> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCustomerKeysAsync(index, length, request);
                return await dataService.DeleteCustomersAsync(items.ToArray());
            }
        }

        static public async Task<CustomerModel> CreateCustomerModelAsync(Customer source, bool includeAllFields)
        {
            var model = new CustomerModel()
            {
                CustomerID = source.CustomerID,
                Title = source.Title,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                LastName = source.LastName,
                Suffix = source.Suffix,
                Gender = source.Gender,
                EmailAddress = source.EmailAddress,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                City = source.City,
                Region = source.Region,
                CountryCode = source.CountryCode,
                PostalCode = source.PostalCode,
                Phone = source.Phone,
                CreatedOn = source.CreatedOn.AsDateTimeOffset(),
                LastModifiedOn = source.LastModifiedOn.AsNullableDateTimeOffset(),
                Thumbnail = source.Thumbnail,
                ThumbnailSource = await BitmapTools.LoadBitmapAsync(source.Thumbnail)
            };
            if (includeAllFields)
            {
                model.BirthDate = source.BirthDate.AsNullableDateTimeOffset();
                model.Education = source.Education;
                model.Occupation = source.Occupation;
                model.YearlyIncome = source.YearlyIncome;
                model.MaritalStatus = source.MaritalStatus;
                model.TotalChildren = source.TotalChildren;
                model.ChildrenAtHome = source.ChildrenAtHome;
                model.IsHouseOwner = source.IsHouseOwner;
                model.NumberCarsOwned = source.NumberCarsOwned;
                model.Picture = source.Picture;
                model.PictureSource = await BitmapTools.LoadBitmapAsync(source.Picture);
            }
            return model;
        }

        private void UpdateCustomerFromModel(Customer target, CustomerModel source)
        {
            target.Title = source.Title;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            target.Gender = source.Gender;
            target.EmailAddress = source.EmailAddress;
            target.AddressLine1 = source.AddressLine1;
            target.AddressLine2 = source.AddressLine2;
            target.City = source.City;
            target.Region = source.Region;
            target.CountryCode = source.CountryCode;
            target.PostalCode = source.PostalCode;
            target.Phone = source.Phone;
            target.BirthDate = source.BirthDate.AsNullableDateTime();
            target.Education = source.Education;
            target.Occupation = source.Occupation;
            target.YearlyIncome = source.YearlyIncome;
            target.MaritalStatus = source.MaritalStatus;
            target.TotalChildren = source.TotalChildren;
            target.ChildrenAtHome = source.ChildrenAtHome;
            target.IsHouseOwner = source.IsHouseOwner;
            target.NumberCarsOwned = source.NumberCarsOwned;
            target.CreatedOn = source.CreatedOn.AsDateTime();
            target.LastModifiedOn = source.LastModifiedOn.AsNullableDateTime();
            target.Picture = source.Picture;
            target.Thumbnail = source.Thumbnail;
        }
    }
}
