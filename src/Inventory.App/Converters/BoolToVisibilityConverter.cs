using System;

using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Inventory.Converters
{
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public object TrueValue { get; set; }
        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = value is bool && (bool)value;

            return XamlBindingHelper.ConvertValue(targetType, boolValue ? TrueValue : FalseValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
