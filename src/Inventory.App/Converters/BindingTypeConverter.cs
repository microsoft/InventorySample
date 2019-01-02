using System;
using Windows.UI.Xaml.Data;

namespace Inventory.Converters
{
    class BindingTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var t = targetType;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return System.Convert.ChangeType(value, t);
        }
    }
}
