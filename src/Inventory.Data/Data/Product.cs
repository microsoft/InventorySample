#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

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
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(4)]
        public string Size { get; set; }
        [MaxLength(50)]
        public string Color { get; set; }

        [Required]
        public decimal ListPrice { get; set; }
        [Required]
        public decimal DealerPrice { get; set; }
        [Required]
        public int TaxType { get; set; }
        [Required]
        public decimal Discount { get; set; }
        public DateTimeOffset? DiscountStartDate { get; set; }
        public DateTimeOffset? DiscountEndDate { get; set; }

        [Required]
        public int StockUnits { get; set; }
        [Required]
        public int SafetyStockLevel { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }
        [Required]
        public DateTimeOffset LastModifiedOn { get; set; }
        public string SearchTerms { get; set; }

        public byte[] Picture { get; set; }
        public byte[] Thumbnail { get; set; }

        public string BuildSearchTerms() => $"{ProductID} {Name} {Color}".ToLower();
    }
}
