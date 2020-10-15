using System;

namespace Alpha.DNAide.Utils
{
    public static class FormatHandler
    {
        public const string CULT_NAME_ENG = "en-US";

        public static string FormatNumber2(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }
            if (culture == "vi-VN")
            {
                return string.Format("{0:#.##0,##}", obj);
            }
            return string.Format("{0:#,##0.##}", obj);
        }

        public static string FormatNumber3(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }
            if (culture == "vi-VN")
            {
                return string.Format("{0:#.##0,###}", obj);
            }
            return string.Format("{0:#,##0.###}", obj);
        }

        public static string FormatNumber4(object obj, string culture)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }
            if (culture == "vi-VN")
            {
                return string.Format("{0:#.##0,####}", obj);
            }
            return string.Format("{0:#,##0.####}", obj);
        }

        public static string FormatDateTimeEn12(object dt)
        {
            string str;
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return "";
            }
            try
            {
                str = Convert.ToDateTime(dt).ToString("MM/dd/yyyy hh:mm:ss tt");
            }
            catch
            {
                str = "Error";
            }
            return str;
        }
    }
}
