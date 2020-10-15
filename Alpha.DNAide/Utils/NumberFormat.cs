namespace Alpha.DNAide.Utils
{
    using DNAide.Core;

    public static class NumberFormat
    {
        public static string DecFormat(decimal value, byte decDigits)
        {
            return value.ToString(string.Concat("N", decDigits));
        }

        public static string DecFormat(object value, byte decDigits)
        {
            return NumberFormat.DecFormat(CastAide.AsDecimal(value, new decimal(0)), decDigits);
        }
    }
}
