namespace Alpha.NetThrottling.WebTest.Controllers
{
    using System.Web.Mvc;

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