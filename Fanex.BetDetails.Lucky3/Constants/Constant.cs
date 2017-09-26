namespace Fanex.BetDetails.Lucky3.Constants
{
    public static class Constant
    {
        public static string MoneyTemplate { get; } = "{0:#,##0.00}";

        public static string CommissionTemplate { get; } = "{0:#,##0.####}";

        public static string DiscountTemplate { get; } = "{0:#,##0.####}%";
        
        public static string NegativeNumberDisplay { get; } = "<font color='#B50000'>{0}</font>";
    }
}