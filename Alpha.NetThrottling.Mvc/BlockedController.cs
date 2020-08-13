namespace Alpha.NetThrottling.Mvc
{
    using Alpha.NetThrottling.Resources;
    using Alpha.NetThrottling.Utils;
    using System.Web.Mvc;

    [DisableNetThrottling]
    [AllowAnonymous]
    public class BlockedController : Controller
    {
        public ActionResult Index()
        {
            Response.StatusCode = HttpStatusCodes.TooManyRequests;
            Response.StatusDescription = Lang.BlockedContent;
            return Content(Lang.BlockedContent);
        }
    }
}
