using Sunplus.Agent.Model.Entities;
using System.Web;

namespace Alpha.NetThrottling.Resolvers
{
    public class HttpErrorInternalResolver : BaseResolver
    {
        public override void BlockNonAjaxRequest(HttpContextBase context)
            => context.Response.Redirect($"~/{UserSession.Current.GetProperty<string>("Report_Routing") ?? string.Empty}error/blocked?path={context.Request.Path}");
    }
}
