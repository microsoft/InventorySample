using System;
using System.Linq.Expressions;

using Inventory.Data;

namespace Inventory.ViewModels
{
    public class OrdersViewState : ListViewState
    {
        static public OrdersViewState CreateDefault() => new OrdersViewState();

        public OrdersViewState()
        {
            OrderByDesc = r => r.OrderDate;
        }

        public long CustomerID { get; set; }

        public Expression<Func<Order, object>> OrderBy { get; set; }
        public Expression<Func<Order, object>> OrderByDesc { get; set; }
    }
}
