namespace Alpha.NetThrottling.Mvc.Resolvers
{
    using Alpha.NetThrottling.Resolvers;
    using System.Web;

    public class DefaultResolver : IResolver
    {
        public void Block(HttpContext context)
        {
            var blockedUrl = $"~/blocked?path={context.Request.Path}";
            context.Response.Redirect(blockedUrl);
        }
    }
}
