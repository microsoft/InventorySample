using System;

using Windows.UI.Xaml.Data;

namespace Inventory.Converters
{
    public sealed class DateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (value is DateTimeOffset dto)
                {
                    return dto;
                }
            }
            return DateTimeOffset.MinValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
