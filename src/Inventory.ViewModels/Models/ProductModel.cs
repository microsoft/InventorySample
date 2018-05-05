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
    public class ProductModel : ObservableObject
    {
        static public ProductModel CreateEmpty() => new ProductModel { ProductID = "", IsEmpty = true };

        public string ProductID { get; set; }

        public int CategoryID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Size { get; set; }
        public string Color { get; set; }

        public decimal ListPrice { get; set; }
        public decimal DealerPrice { get; set; }
        public int TaxType { get; set; }
        public decimal Discount { get; set; }
        public DateTimeOffset? DiscountStartDate { get; set; }
        public DateTimeOffset? DiscountEndDate { get; set; }

        public int StockUnits { get; set; }
        public int SafetyStockLevel { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }

        public byte[] Picture { get; set; }
        public object PictureSource { get; set; }

        public byte[] Thumbnail { get; set; }
        public object ThumbnailSource { get; set; }

        public bool IsNew => String.IsNullOrEmpty(ProductID);
        public string CategoryName => LookupTablesProxy.Instance.GetCategory(CategoryID);

        public override void Merge(ObservableObject source)
        {
            if (source is ProductModel model)
            {
                Merge(model);
            }
        }

        public void Merge(ProductModel source)
        {
            if (source != null)
            {
                ProductID = source.ProductID;
                CategoryID = source.CategoryID;
                Name = source.Name;
                Description = source.Description;
                Size = source.Size;
                Color = source.Color;
                ListPrice = source.ListPrice;
                DealerPrice = source.DealerPrice;
                TaxType = source.TaxType;
                Discount = source.Discount;
                DiscountStartDate = source.DiscountStartDate;
                DiscountEndDate = source.DiscountEndDate;
                StockUnits = source.StockUnits;
                SafetyStockLevel = source.SafetyStockLevel;
                CreatedOn = source.CreatedOn;
                LastModifiedOn = source.LastModifiedOn;
                Picture = source.Picture;
                PictureSource = source.PictureSource;
                Thumbnail = source.Thumbnail;
                ThumbnailSource = source.ThumbnailSource;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
