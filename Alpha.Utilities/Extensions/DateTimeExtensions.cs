namespace Alpha.Utilities.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public const string ShortDateFormat = "MMM dd, yyyy";
        public const string MonthYearFormat = "MMM yyyy";
        public const string ShortDateFormatWithSlash = "MM/dd/yyyy";
        public const string LongDateFormatWithSlash = "MM/dd/yyyy hh:mm:ss tt";
        public const string MonthYearFormatWithSlash = "MM/yyyy";
        public const string LongDateFormat = "dd MMM, yyyy hh:mm:ss tt";
        public const string AresDateFormat = "dd-MMM-yy";
        public const string AresMonthYearFormat = "MMM-yy";
        public const string MonthAndDateFormat = "dd-MMM";

        /// <summary>
        /// Get First Date of Month
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Get Last Date Of Month
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Format Short Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToShortDateFormat(this DateTime? date)
        {
            if (date.HasValue)
            {
                return ToShortDateFormat(date.Value);
            }

            return string.Empty;
        }

        /// <summary>
        /// format a datetime value to string (MMM dd, yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToShortDateFormat(this DateTime date)
        {
            return date.ToString(ShortDateFormat);
        }

        /// <summary>
        /// format a nullable datetime value to string (MMM dd, yyyy hh:mm)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToLongDateFormat(this DateTime? date)
        {
            if (date.HasValue)
            {
                return ToLongDateFormat(date.Value);
            }

            return string.Empty;
        }

        /// <summary>
        /// format a datetime value to string (MMM dd, yyyy hh:mm)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToLongDateFormat(this DateTime date)
        {
            return date.ToString(LongDateFormat);
        }

        /// <summary>
        /// Format a nullable datetime value to string (dd-MMM)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToMonthAndDayFormat(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthAndDayFormat();
            }

            return string.Empty;
        }

        /// <summary>
        /// Foarmat da datatime value to string (dd-MMM)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToMonthAndDayFormat(this DateTime date)
        {
            return date.ToString(MonthAndDateFormat);
        }

        /// <summary>
        /// Format a nullable datetime value to string (MMM yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToMonthYearFormat(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthYearFormat();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format datatime value to string
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string with format (MMM yyyy)</returns>
        public static string ToMonthYearFormat(this DateTime date)
        {
            return date.ToString(MonthYearFormat);
        }

        /// <summary>
        /// Format Short Date (MM/dd/yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToShortDateFormatWithSlash(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortDateFormatWithSlash();
            }

            return string.Empty;
        }

        /// <summary>
        /// format a datetime value to string (MM/dd/yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToLongDateFormatWithSlash(this DateTime date)
        {
            return date.ToString(LongDateFormatWithSlash);
        }

        /// <summary>
        /// format a datetime value to string (MM/dd/yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToShortDateFormatWithSlash(this DateTime date)
        {
            return date.ToString(ShortDateFormatWithSlash);
        }

        /// <summary>
        /// Format a nullable datetime value to string (MM/yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToMonthYearFormatWithSlash(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthYearFormatWithSlash();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format datatime value to string
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string with format (MM/yyyy)</returns>
        public static string ToMonthYearFormatWithSlash(this DateTime date)
        {
            return date.ToString(MonthYearFormatWithSlash);
        }

        /// <summary>
        /// Foarmat da datatime value to string (dd-MMM-yy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToAresDayMonthYearFormat(this DateTime date)
        {
            return date.ToString(AresDateFormat);
        }

        /// <summary>
        /// Format datatime value to string (MMM-yy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToAresMonthYearFormat(this DateTime date)
        {
            return date.ToString(AresMonthYearFormat);
        }

        /// <summary>
        /// Convert string yyyyMMdd to DateTime object
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string date)
        {
            try
            {
                return Convert.ToDateTime(date.Substring(4, 2) + "/" + date.Substring(6, 2) + "/" + date.Substring(0, 4));
            }
            catch
            {
                return new DateTime();
            }
        }
    }
}