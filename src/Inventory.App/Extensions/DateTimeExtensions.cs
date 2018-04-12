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
