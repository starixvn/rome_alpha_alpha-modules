namespace Alpha.NetThrottling.WebTest.Controllers
{
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        public ActionResult Blocked()
        {
            return View();
        }
    }
}