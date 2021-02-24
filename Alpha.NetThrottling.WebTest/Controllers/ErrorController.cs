namespace Alpha.NetThrottling.WebTest.Controllers
{
    using Alpha.NetThrottling.Mvc;
    using System.Web.Mvc;

    [DisableNetThrottling]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Blocked()
        {
            return View();
        }
    }
}