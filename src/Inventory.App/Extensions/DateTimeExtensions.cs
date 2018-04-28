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

namespace Inventory
{
    static public class DateTimeExtensions
    {
        static public DateTimeOffset AsDateTimeOffset(this DateTime dateTime)
        {
            long ticks = Math.Max(DateTimeOffset.MinValue.Ticks, dateTime.Ticks);
            return new DateTimeOffset(ticks, TimeSpan.Zero);
        }
        static public DateTimeOffset? AsNullableDateTimeOffset(this DateTime? dateTime)
        {
            if (dateTime != null)
            {
                return AsDateTimeOffset(dateTime.Value);
            }
            return null;
        }

        static public DateTime AsDateTime(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.DateTime;
        }
        static public DateTime? AsNullableDateTime(this DateTimeOffset? dateTimeOffset)
        {
            if (dateTimeOffset != null)
            {
                return dateTimeOffset.Value.DateTime;
            }
            return null;
        }
    }
}
