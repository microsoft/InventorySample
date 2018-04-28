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
                if (value is DateTime dateTime)
                {
                    if (dateTime == DateTime.MinValue)
                    {
                        return "";
                    }
                    value = new DateTimeOffset(dateTime);
                }
                if (value is DateTimeOffset dateTimeOffset)
                {
                    string format = parameter as String ?? "shortdate";
                    var userLanguages = GlobalizationPreferences.Languages;
                    var dateFormatter = new DateTimeFormatter(format, userLanguages);
                    return dateFormatter.Format(dateTimeOffset.ToLocalTime());
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
