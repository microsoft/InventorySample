using System;

using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

using Inventory.Data;

namespace Inventory.Converters
{
    public sealed class LogTypeConverter : IValueConverter
    {
        private readonly SolidColorBrush InformationColor = new SolidColorBrush(Colors.Navy);
        private readonly SolidColorBrush WarningColor = new SolidColorBrush(Colors.Gold);
        private readonly SolidColorBrush ErrorColor = new SolidColorBrush(Colors.IndianRed);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(String))
            {
                if (value is LogType logType)
                {
                    switch (logType)
                    {
                        case LogType.Information:
                            return Char.ConvertFromUtf32(0xE946).ToString();
                        case LogType.Warning:
                            return Char.ConvertFromUtf32(0xE814).ToString();
                        case LogType.Error:
                            return Char.ConvertFromUtf32(0xEB90).ToString();
                    }
                }
                return Char.ConvertFromUtf32(0xE946).ToString();
            }

            if (targetType == typeof(Brush))
            {
                if (value is LogType logType)
                {
                    switch (logType)
                    {
                        case LogType.Information:
                            return InformationColor;
                        case LogType.Warning:
                            return WarningColor;
                        case LogType.Error:
                            return ErrorColor;
                    }
                }
                return InformationColor;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
