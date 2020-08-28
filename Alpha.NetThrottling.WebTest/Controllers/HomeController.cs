namespace Alpha.NetThrottling.WebTest.Controllers
{
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using Alpha.NetThrottling.WebTest.Constants;
    using Alpha.NetThrottling.WebTest.Models;
    using Alpha.NetThrottling.WebTest.Services;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private readonly LoginService loginService;
        private readonly INetThrottler netThrottler;
        private readonly IThrottlePolicy throttlePolicy;

        public HomeController() :
            this(null, null)
        {
        }

        public HomeController(LoginService loginService, IThrottleSetting throttleSetting)
        {
            this.loginService = loginService ?? new LoginService();
            throttleSetting = throttleSetting ?? new ThrottleSetting("Alpha.NetThrottling.Manual");
            netThrottler = new NetThrottler(throttleSetting);
            throttlePolicy = new ThrottlePolicy(throttleSetting.GetKey("Login"));
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = "Your login page.";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var filterIdentity = $"{model.Username}_{Request.GetClientIp()}";
            return await netThrottler.ThrottleAsync<ActionResult>(
                async (success) =>
                {
                    var user = loginService.Login(model);
                    if (user != null)
                    {
                        Session[SessionConstants.USER] = user;
                        return await success(RedirectToAction("Index", "Home"));
                    }
                    else
                    {
                        ViewBag.Message = "Wrong password";
                        return View();
                    }
                },
                () => RedirectToAction("Blocked", "Error"),
                throttlePolicy,
                filterIdentity
            );
        }
    }
}