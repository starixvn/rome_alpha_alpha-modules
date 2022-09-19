using System;
using System.Web;

namespace Alpha.NetThrottling.Resolvers
{
    public class HttpErrorInternalResolver : BaseResolver
    {
        public override void BlockNonAjaxRequest(HttpContextBase context)
        {
            var currentAppPath = HttpRuntime.AppDomainAppVirtualPath;
            var isCookieless = context.Session.IsCookieless;
            var routing = !isCookieless && currentAppPath.Equals("/", StringComparison.OrdinalIgnoreCase) ?
                "site-reports/" : string.Empty;

            context.Response.Redirect($"~/{routing}error/blocked?path={context.Request.Path}");
        }
    }
}
