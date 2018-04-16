using System;

namespace Inventory.Models
{
    public class SubCategoryModel : ModelBase
    {
        public int CategoryID { get; set; }

        public int SubCategoryID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
