using System;

namespace Inventory.ViewModels
{
    public class CustomerViewState : DetailsViewState
    {
        static public CustomerViewState CreateDefault() => new CustomerViewState();

        public long CustomerID { get; set; }

        public bool IsNew => CustomerID <= 0;
    }
}
