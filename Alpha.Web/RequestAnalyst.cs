namespace Alpha.Web
{
    using System.Text.RegularExpressions;
    using Fanex.Common;

    public class RequestAnalyst
    {
        public static int GetCustomerId(string jsonString)
        {
            var customerId = 0;
            if (!string.IsNullOrEmpty(jsonString))
            {
                var pattern = "({\"custId\":)([0-9]+)(,)";
                Match regexMatch = Regex.Match(jsonString, pattern, RegexOptions.IgnoreCase);
                if (regexMatch.Success)
                {
                    customerId = Converter.ToInt(regexMatch.Groups[2]);
                }
            }

            return customerId;
        }
    }
}