using Alpha.ActivityTracking.Models;
using Alpha.ActivityTracking.Services;
using Fanex.ActivityTracking.Web;

namespace Alpha.ActivityTracking.Web
{
    public class AlphaActivityTrackingHttpModule : ActivityTrackingHttpModule
    {
        public AlphaActivityTrackingHttpModule()
             : base(new AlphaUserSession(), new RequestAnalyzer(), new JsonConverter())
        {
        }
    }
}
