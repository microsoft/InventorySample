using System;

using Windows.UI.Xaml.Media.Imaging;

namespace Inventory.Models
{
    public class CategoryModel : ModelBase
    {
        public int CategoryID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public byte[] Picture { get; set; }
        public BitmapImage PictureBitmap { get; set; }

        public byte[] Thumbnail { get; set; }
        public BitmapImage ThumbnailBitmap { get; set; }
    }
}
