using System;

namespace Inventory.ViewModels
{
    public class OrderItemViewState
    {
        static public OrderItemViewState CreateDefault() => new OrderItemViewState(0);

        public OrderItemViewState(long orderID)
        {
            OrderID = orderID;
        }

        public long OrderID { get; set; }
        public int OrderLine { get; set; }

        public bool IsNew => OrderLine <= 0;
    }
}
