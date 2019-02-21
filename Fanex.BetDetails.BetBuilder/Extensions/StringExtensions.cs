namespace Fanex.BetDetails.BetBuilder.Extensions
{
    using System.Linq;

    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.First().ToString().ToUpper() + 
                (value.Length > 1 ? value.Substring(1).ToLower() : string.Empty);
        }
    }
}
