using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    [Table("Products")]
    public partial class Product
    {
        [MaxLength(16)]
        [Key, Column(Order = 0)]
        public string ProductID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required]
        public int SubCategoryID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(4)]
        public string Size { get; set; }
        [MaxLength(50)]
        public string Color { get; set; }
        [MaxLength(1)]
        public string Gender { get; set; }

        [Required]
        public decimal ListPrice { get; set; }
        [Required]
        public decimal DealerPrice { get; set; }
        [Required]
        public int TaxType { get; set; }
        [Required]
        public decimal Discount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }

        [Required]
        public int StockUnits { get; set; }
        [Required]
        public int SafetyStockLevel { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime LastModifiedOn { get; set; }
        public string SearchTerms { get; set; }

        public byte[] Picture { get; set; }
        public byte[] Thumbnail { get; set; }

        public string BuildSearchTerms() => $"{ProductID} {Name} {Color}".ToLower();
    }
}
