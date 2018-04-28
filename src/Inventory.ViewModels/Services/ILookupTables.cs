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
