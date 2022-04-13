using System.Configuration;
using System.Web;

namespace Alpha.NetThrottling.Resolvers
{
    public class HttpErrorInternalResolver : BaseResolver
    {
        public override void BlockNonAjaxRequest(HttpContextBase context)
            => context.Response.Redirect($"~/{ConfigurationManager.AppSettings["Report:Routing"]}error/blocked?path={context.Request.Path}");
    }
}
