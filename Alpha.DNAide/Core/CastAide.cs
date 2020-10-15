namespace Alpha.DNAide.Core
{
    using System;
    using System.Globalization;

    public static class CastAide
    {
        public static bool AsBoolean(object value)
        {
            return CastAide.AsBoolean(value, false, CultureInfo.CurrentCulture);
        }

        public static bool AsBoolean(object value, bool defaultValue, IFormatProvider provider)
        {
            bool flag;
            if (value == null || value == DBNull.Value)
            {
                return defaultValue;
            }
            if (provider == null)
            {
                provider = CultureInfo.CurrentCulture;
            }
            try
            {
                flag = Convert.ToBoolean(value, provider);
            }
            catch (InvalidCastException invalidCastException)
            {
                flag = defaultValue;
            }
            catch (FormatException formatException)
            {
                flag = defaultValue;
            }
            return flag;
        }

        public static int AsInt32(object value)
        {
            return CastAide.AsInt32(value, 0, CultureInfo.CurrentCulture);
        }

        public static int AsInt32(object value, int defaultValue)
        {
            return CastAide.AsInt32(value, defaultValue, CultureInfo.CurrentCulture);
        }

        public static int AsInt32(object value, int defaultValue, IFormatProvider provider)
        {
            int num;
            if (value == null || value == DBNull.Value)
            {
                return defaultValue;
            }
            if (provider == null)
            {
                provider = CultureInfo.CurrentCulture;
            }
            try
            {
                num = Convert.ToInt32(value, provider);
            }
            catch (InvalidCastException invalidCastException)
            {
                num = defaultValue;
            }
            catch (FormatException formatException)
            {
                num = defaultValue;
            }
            return num;
        }

        public static double AsDouble(object value)
        {
            return CastAide.AsDouble(value, 0, CultureInfo.CurrentCulture);
        }

        public static double AsDouble(object value, double defaultValue, IFormatProvider provider)
        {
            double num;
            if (value == null || value == DBNull.Value)
            {
                return defaultValue;
            }
            if (provider == null)
            {
                provider = CultureInfo.CurrentCulture;
            }
            try
            {
                num = Convert.ToDouble(value, provider);
            }
            catch (InvalidCastException invalidCastException)
            {
                num = defaultValue;
            }
            catch (FormatException formatException)
            {
                num = defaultValue;
            }
            return num;
        }

        public static string AsString(object value)
        {
            return CastAide.AsString(value, string.Empty, CultureInfo.CurrentCulture);
        }

        public static string AsString(object value, string defaultValue)
        {
            return CastAide.AsString(value, defaultValue, CultureInfo.CurrentCulture);
        }

        public static string AsString(object value, string defaultValue, IFormatProvider provider)
        {
            string str;
            if (value == null || value == DBNull.Value)
            {
                return defaultValue;
            }
            if (provider == null)
            {
                provider = CultureInfo.CurrentCulture;
            }
            try
            {
                str = Convert.ToString(value, provider);
            }
            catch (InvalidCastException invalidCastException)
            {
                str = defaultValue;
            }
            return str;
        }

        public static decimal AsDecimal(object value)
        {
            return CastAide.AsDecimal(value, new decimal(0), CultureInfo.CurrentCulture);
        }

        public static decimal AsDecimal(object value, decimal defaultValue)
        {
            return CastAide.AsDecimal(value, defaultValue, CultureInfo.CurrentCulture);
        }

        public static decimal AsDecimal(object value, decimal defaultValue, IFormatProvider provider)
        {
            decimal num;
            if (value == null || value == DBNull.Value)
            {
                return defaultValue;
            }
            if (provider == null)
            {
                provider = CultureInfo.CurrentCulture;
            }
            try
            {
                num = Convert.ToDecimal(value, provider);
            }
            catch (InvalidCastException invalidCastException)
            {
                num = defaultValue;
            }
            catch (FormatException formatException)
            {
                num = defaultValue;
            }
            return num;
        }

        public static short AsInt16(object value)
        {
            return CastAide.AsInt16(value, 0, CultureInfo.CurrentCulture);
        }

        public static short AsInt16(object value, short defaultValue, IFormatProvider provider)
        {
            short num;
            if (value == null || value == DBNull.Value)
            {
                return defaultValue;
            }
            if (provider == null)
            {
                provider = CultureInfo.CurrentCulture;
            }
            try
            {
                num = Convert.ToInt16(value, provider);
            }
            catch (InvalidCastException invalidCastException)
            {
                num = defaultValue;
            }
            catch (FormatException formatException)
            {
                num = defaultValue;
            }
            return num;
        }
    }
}
