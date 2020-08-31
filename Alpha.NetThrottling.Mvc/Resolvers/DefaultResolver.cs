namespace Alpha.NetThrottling.Mvc.Resolvers
{
    using Alpha.NetThrottling.Resolvers;
    using System.Web;

    public class DefaultResolver : BaseResolver
    {
        public override void BlockNonAjaxRequest(HttpContextBase context)
        {
            var blockedUrl = $"~/blocked?path={context.Request.Path}";
            context.Response.Redirect(blockedUrl);
        }
    }
}
