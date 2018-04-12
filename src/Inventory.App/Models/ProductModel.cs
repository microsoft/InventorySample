using System;

using Windows.UI.Xaml.Media.Imaging;

namespace Inventory.Models
{
    public class ProductModel : ModelBase
    {
        static public ProductModel CreateEmpty() => new ProductModel { ProductID = "", IsEmpty = true };

        public string ProductID { get; set; }

        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Size { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }

        public decimal ListPrice { get; set; }
        public decimal DealerPrice { get; set; }
        public int TaxType { get; set; }
        public decimal Discount { get; set; }
        public DateTimeOffset? DiscountStartDate { get; set; }
        public DateTimeOffset? DiscountEndDate { get; set; }

        public int StockUnits { get; set; }
        public int SafetyStockLevel { get; set; }

        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }

        public byte[] Picture { get; set; }
        public BitmapImage PictureBitmap { get; set; }

        public byte[] Thumbnail { get; set; }
        public BitmapImage ThumbnailBitmap { get; set; }

        public bool IsNew => String.IsNullOrEmpty(ProductID);
        public string CategoryName => DataHelper.GetCategory(CategoryID);

        public override string ToString()
        {
            return Name;
        }
    }
}
