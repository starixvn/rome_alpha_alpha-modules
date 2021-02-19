namespace Alpha.NetThrottling.Mvc.Resolvers
{
    using Alpha.NetThrottling.Resolvers;
    using Alpha.Web.Helpers;
    using System.Web;

    public class DefaultResolver : BaseResolver
    {
        public override void BlockNonAjaxRequest(HttpContextBase context)
        {
            var blockedUrl = MicroSites.MainUrl($"~/error/blocked?path={context.Request.Path}");
            context.Response.Redirect(blockedUrl);
        }
    }
}
