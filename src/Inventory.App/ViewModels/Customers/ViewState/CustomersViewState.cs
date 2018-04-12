using System;
using System.Linq.Expressions;

using Inventory.Data;

namespace Inventory.ViewModels
{
    public class CustomersViewState : ListViewState
    {
        static public CustomersViewState CreateDefault() => new CustomersViewState();

        public CustomersViewState()
        {
            OrderBy = r => r.FirstName;
        }

        public Expression<Func<Customer, object>> OrderBy { get; set; }
        public Expression<Func<Customer, object>> OrderByDesc { get; set; }
    }
}
