using System;

namespace Inventory.Models
{
    public class CategoryModel : ModelBase
    {
        public int CategoryID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public byte[] Thumbnail { get; set; }
    }
}
