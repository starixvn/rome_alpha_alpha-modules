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
        private readonly IThrottleSetting throttleSetting;
        private readonly IThrottlePolicy throttlePolicy;

        public HomeController() :
            this(null, null, null)
        {
        }

        public HomeController(LoginService loginService, IThrottleSetting throttleSetting, IThrottlePolicy throttlePolicy)
        {
            this.loginService = loginService ?? new LoginService();
            this.throttleSetting = throttleSetting ?? new ThrottleSetting(NetThrottlingConfiguration.Current);
            this.throttlePolicy = throttlePolicy ?? new ThrottlePolicy(this.throttleSetting.GetKey("Login"));
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
            var netThrottler = new NetThrottler(throttleSetting, null, null);
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