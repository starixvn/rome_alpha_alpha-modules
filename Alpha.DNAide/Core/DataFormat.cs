namespace Alpha.DNAide.Core
{
    using System;

    public class DataFormat
    {
        public static string FormatNumber20(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }
            return string.Format("{0:#,##0.00}", obj);
        }
    }
}
