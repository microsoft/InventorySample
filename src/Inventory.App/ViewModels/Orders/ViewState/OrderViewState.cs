using System;

namespace Inventory.ViewModels
{
    public class OrderViewState
    {
        static public OrderViewState CreateDefault() => new OrderViewState(0);

        public OrderViewState(long customerID)
        {
            CustomerID = customerID;
        }

        public long CustomerID { get; set; }
        public long OrderID { get; set; }

        public bool IsNew => OrderID <= 0;
    }
}
