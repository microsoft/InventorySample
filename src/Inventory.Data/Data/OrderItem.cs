using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    [Table("OrderItems")]
    public partial class OrderItem
    {
        [Key]
        [DatabaseGenerat‌​ed(DatabaseGeneratedOption.None)]
        public long OrderID { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerat‌​ed(DatabaseGeneratedOption.None)]
        public int OrderLine { get; set; }

        [Required]
        [MaxLength(16)]
        public string ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal Discount { get; set; }
        [Required]
        public int TaxType { get; set; }

        public virtual Product Product { get; set; }
    }
}
