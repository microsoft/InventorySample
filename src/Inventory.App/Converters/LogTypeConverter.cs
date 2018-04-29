#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

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
