using Fanex.ActivityTracking.Web;

namespace Alpha.ActivityTracking.Services
{
    public class RequestAnalyzer : BaseRequestAnalyzer
    {
        public RequestAnalyzer() : base(new JsonConverter())
        {
        }
    }
}
