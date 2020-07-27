namespace Alpha.NetThrottling.Resolvers
{
    using System.Web;

    public interface IResolver
    {
        void Block(HttpContext context);
    }
}
