namespace Alpha.NetThrottling.WebTest.Controllers
{
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Mvc;
    using Alpha.NetThrottling.WebTest.Constants;
    using Alpha.NetThrottling.WebTest.Models;
    using Alpha.NetThrottling.WebTest.Services;
    using Alpha.NetThrottling.WebTest.ThrottlingFilters;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private readonly LoginService loginService;
        private readonly IThrottleSetting loginThrottleSetting;
        private readonly INetThrottler loginNetThrottler;
        private readonly IThrottlePolicy loginThrottlePolicy;

        public HomeController() :
            this(null, null)
        {
        }

        public HomeController(LoginService loginService, IThrottleSetting loginThrottleSetting)
        {
            this.loginService = loginService ?? new LoginService();
            this.loginThrottleSetting = loginThrottleSetting ?? new ThrottleSetting("Alpha.NetThrottling.Manual");
            loginNetThrottler = new NetThrottler(this.loginThrottleSetting);
            loginThrottlePolicy = new ThrottlePolicy(this.loginThrottleSetting.GetKey("Login"));
        }

        [DisableNetThrottling]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Ajax(string username)
        {
            return Json(new
            {
                username
            });
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
        public ActionResult Login(LoginModel model)
        {
            var currentHttpContext = System.Web.HttpContext.Current;
            User loginUser = null;
            var filter = new LoginFilter();
            var result = loginNetThrottler.ThrottleAsync(
                async (success) =>
                {
                    System.Web.HttpContext.Current = currentHttpContext;
                    loginUser = loginService.Login(model);
                    if (loginUser != null)
                    {
                        return await success(ThrottleResult.Allowed);
                    }
                    else
                    {
                        return ThrottleResult.Allowed;
                    }
                },
                () => ThrottleResult.Blocked,
                loginThrottlePolicy,
                filter.GetCacheIdentity(HttpContext, loginThrottlePolicy)
            ).Result;
            if (result == ThrottleResult.Blocked)
            {
                ViewBag.Message = "Too many requests";
                return View();
            }

            if (loginUser == null)
            {
                ViewBag.Message = "Wrong password";
                return View();

            }

            Session[SessionConstants.USER] = loginUser;
            return RedirectToAction("Index", "Home");
        }
    }
}