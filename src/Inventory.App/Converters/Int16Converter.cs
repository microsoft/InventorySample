using System;

using Windows.UI.Xaml.Data;

namespace Inventory.Converters
{
    public sealed class Int16Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Int16 n16)
            {
                if (targetType == typeof(String))
                {
                    return n16 == 0 ? "" : n16.ToString();
                }
                return n16;
            }
            if (targetType == typeof(String))
            {
                return "";
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (Int16.TryParse(value.ToString(), out Int16 n16))
                {
                    return n16;
                }
            }
            return 0;
        }
    }
}
