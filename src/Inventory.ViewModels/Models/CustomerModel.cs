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

using Inventory.Services;

namespace Inventory.Models
{
    public class CustomerModel : ObservableObject
    {
        static public CustomerModel CreateEmpty() => new CustomerModel { CustomerID = -1, IsEmpty = true };

        public long CustomerID { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Gender { get; set; }

        public string EmailAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }

        public DateTimeOffset? BirthDate { get; set; }
        public string Education { get; set; }
        public string Occupation { get; set; }
        public decimal? YearlyIncome { get; set; }
        public string MaritalStatus { get; set; }
        public int? TotalChildren { get; set; }
        public int? ChildrenAtHome { get; set; }
        public bool? IsHouseOwner { get; set; }
        public int? NumberCarsOwned { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? LastModifiedOn { get; set; }

        public byte[] Picture { get; set; }
        public object PictureSource { get; set; }

        public byte[] Thumbnail { get; set; }
        public object ThumbnailSource { get; set; }

        public bool IsNew => CustomerID <= 0;
        public string FullName => $"{FirstName} {LastName}";
        public string Initials => String.Format("{0}{1}", $"{FirstName} "[0], $"{LastName} "[0]).Trim().ToUpper();
        public string CountryName => LookupTablesProxy.Instance.GetCountry(CountryCode);

        public string FullAddress
        {
            get
            {
                return $"{AddressLine1} {AddressLine2}\n{City}, {Region} {PostalCode}\n{CountryName}";
          
            }
        }

        public override void Merge(ObservableObject source)
        {
            if (source is CustomerModel model)
            {
                Merge(model);
            }
        }

        public void Merge(CustomerModel source)
        {
            if (source != null)
            {
                CustomerID = source.CustomerID;
                Title = source.Title;
                FirstName = source.FirstName;
                MiddleName = source.MiddleName;
                LastName = source.LastName;
                Suffix = source.Suffix;
                Gender = source.Gender;
                EmailAddress = source.EmailAddress;
                AddressLine1 = source.AddressLine1;
                AddressLine2 = source.AddressLine2;
                City = source.City;
                Region = source.Region;
                CountryCode = source.CountryCode;
                PostalCode = source.PostalCode;
                Phone = source.Phone;
                BirthDate = source.BirthDate;
                Education = source.Education;
                Occupation = source.Occupation;
                YearlyIncome = source.YearlyIncome;
                MaritalStatus = source.MaritalStatus;
                TotalChildren = source.TotalChildren;
                ChildrenAtHome = source.ChildrenAtHome;
                IsHouseOwner = source.IsHouseOwner;
                NumberCarsOwned = source.NumberCarsOwned;
                CreatedOn = source.CreatedOn;
                LastModifiedOn = source.LastModifiedOn;
                Thumbnail = source.Thumbnail;
                ThumbnailSource = source.ThumbnailSource;
                Picture = source.Picture;
                PictureSource = source.PictureSource;
            }
        }

        public override string ToString()
        {
            return IsEmpty ? "Empty" : FullName;
        }
    }
}
