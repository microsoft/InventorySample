using System;

namespace Inventory.Models
{
    public class TaxTypeModel : ModelBase
    {
        public int TaxTypeID { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
    }
}
