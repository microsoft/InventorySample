using System;

using Windows.UI.Xaml.Data;
using Windows.System.UserProfile;
using Windows.Globalization.DateTimeFormatting;

namespace Inventory.Converters
{
    public sealed class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is DateTimeOffset dateTime)
                {
                    string format = parameter as String ?? "shortdate";
                    var userLanguages = GlobalizationPreferences.Languages;
                    var dateFormatter = new DateTimeFormatter(format, userLanguages);
                    return dateFormatter.Format(dateTime);
                }
                return "N/A";
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
