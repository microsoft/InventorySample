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
