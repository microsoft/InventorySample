using System;

namespace Inventory.Models
{
    public class OrderStatusModel : ModelBase
    {
        public int Status { get; set; }
        public string Name { get; set; }
    }
}
