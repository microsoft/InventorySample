using System;

using Windows.UI.Xaml.Data;

namespace Inventory.Converters
{
    public sealed class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double d)
            {
                return d == 0.0 ? "" : d.ToString("0.00");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (Double.TryParse(value.ToString(), out double d))
                {
                    return d;
                }
            }
            return 0.0;
        }
    }
}
