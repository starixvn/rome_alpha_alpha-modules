namespace Alpha.NetThrottling.Utils
{
    using System.Web;

    public static class ConfigurationExtensions
    {
        public static bool IsMatchRequest(this NetThrottlingKeyElement key, HttpRequest request)
        {
            var isPathMatched = key.Path == "*" || key.Path == request.AppRelativeCurrentExecutionFilePath;
            var isMethodMatched = key.Method == "*" || key.Method == request.HttpMethod;
            return isPathMatched && isMethodMatched;
        }
    }
}
