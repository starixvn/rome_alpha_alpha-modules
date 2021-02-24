namespace Alpha.NetThrottling.Resolvers
{
    using Alpha.NetThrottling.Resources;
    using Alpha.NetThrottling.Utils;
    using Alpha.Web.Helpers;
    using System.Web;

    public class HttpErrorResolver : BaseResolver
    {
        public override void BlockNonAjaxRequest(HttpContextBase context)
        {
            var blockedUrl = MicroSites.MainUrl($"~/error/blocked?path={context.Request.Path}");
            context.Response.Redirect(blockedUrl);
        }
    }
}
