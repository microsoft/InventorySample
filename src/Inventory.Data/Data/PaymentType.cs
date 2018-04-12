using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    [Table("PaymentTypes")]
    public partial class PaymentType
    {
        [Key]
        [DatabaseGenerat‌​ed(DatabaseGeneratedOption.None)]
        public int PaymentTypeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
