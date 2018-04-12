using System;

using Windows.UI.Xaml.Data;
using Windows.System.UserProfile;
using Windows.Globalization.DateTimeFormatting;
using System.Globalization;

namespace Inventory.Converters
{
    public sealed class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dateTime)
            {
                try
                {
                    string format = parameter as String ?? "shortdate";
                    var userLanguages = GlobalizationPreferences.Languages;
                    var dateFormatter = new DateTimeFormatter(format, userLanguages);
                    return dateFormatter.Format(dateTime);
                }
                catch
                {
                    return "N/A";
                }
            }
            return "N/A";
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
