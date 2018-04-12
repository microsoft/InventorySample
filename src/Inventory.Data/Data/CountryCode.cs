using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    [Table("CountryCodes")]
    public partial class CountryCode
    {
        [MaxLength(2)]
        [Key]
        public string CountryCodeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
