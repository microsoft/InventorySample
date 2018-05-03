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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    [Table("Customers")]
    public partial class Customer
    {
        [Key]
        [DatabaseGenerat‌​ed(DatabaseGeneratedOption.None)]
        public long CustomerID { get; set; }

        [MaxLength(8)]
        public string Title { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(10)]
        public string Suffix { get; set; }
        [MaxLength(1)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(120)]
        public string AddressLine1 { get; set; }
        [MaxLength(120)]
        public string AddressLine2 { get; set; }
        [Required]
        [MaxLength(30)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string Region { get; set; }
        [Required]
        [MaxLength(2)]
        public string CountryCode { get; set; }
        [Required]
        [MaxLength(15)]
        public string PostalCode { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }

        public DateTimeOffset? BirthDate { get; set; }
        [MaxLength(40)]
        public string Education { get; set; }
        [MaxLength(100)]
        public string Occupation { get; set; }
        public decimal? YearlyIncome { get; set; }
        [MaxLength(1)]
        public string MaritalStatus { get; set; }
        public int? TotalChildren { get; set; }
        public int? ChildrenAtHome { get; set; }
        public bool? IsHouseOwner { get; set; }
        public int? NumberCarsOwned { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }
        [Required]
        public DateTimeOffset? LastModifiedOn { get; set; }
        public string SearchTerms { get; set; }

        public byte[] Picture { get; set; }
        public byte[] Thumbnail { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public string BuildSearchTerms() => $"{CustomerID} {FirstName} {LastName} {EmailAddress} {AddressLine1}".ToLower();
    }
}
