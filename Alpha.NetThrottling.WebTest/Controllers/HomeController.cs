namespace Alpha.NetThrottling.WebTest.Controllers
{
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.WebTest.Constants;
    using Alpha.NetThrottling.WebTest.Models;
    using Alpha.NetThrottling.WebTest.Services;
    using Alpha.NetThrottling.WebTest.ThrottlingFilters;
    using System.Threading.Tasks;
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

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Ajax(string username)
        {
            return Json(new {
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
        public async Task<ActionResult> Login(LoginModel model)
        {
            return await loginNetThrottler.ThrottleAsync<ActionResult>(
                HttpContext,
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
                () =>
                {
                    loginThrottleSetting.Resolver.Block(HttpContext);
                    return View();
                },
                loginThrottlePolicy,
                new LoginFilter()
            );
        }
    }
}