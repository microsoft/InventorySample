using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;

namespace Inventory.Services
{
    public interface ILookupTables
    {
        Task InitializeAsync();

        IList<CategoryModel> Categories { get; }
        IList<CountryCodeModel> CountryCodes { get; }
        IList<OrderStatusModel> OrderStatus { get; }
        IList<PaymentTypeModel> PaymentTypes { get; }
        IList<ShipperModel> Shippers { get; }
        IList<TaxTypeModel> TaxTypes { get; }

        string GetCategory(int id);
        string GetCountry(string id);
        string GetOrderStatus(int id);
        string GetPaymentType(int? id);
        string GetShipper(int? id);
        string GetTaxDesc(int id);
        decimal GetTaxRate(int id);
    }

    public class LookupTablesProxy
    {
        static public ILookupTables Instance { get; set; }
    }
}
