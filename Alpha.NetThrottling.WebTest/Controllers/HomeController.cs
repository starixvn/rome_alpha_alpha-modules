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
        private static ThrottlePolicy configurationLoginPolicy = Mapper.From(NetThrottlingConfiguration.Current.Keys["Login"]);
        private readonly LoginService loginService;
        private readonly ThrottlePolicy loginPolicy;

        public HomeController() :
            this(null, null)
        {
        }

        public HomeController(LoginService loginService, ThrottlePolicy loginPolicy)
        {
            this.loginService = loginService ?? new LoginService();
            this.loginPolicy = loginPolicy ?? configurationLoginPolicy;
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
            var netThrottler = new NetThrottler();
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
                () => RedirectToAction("AccessDenied", "Home"),
                filterIdentity,
                loginPolicy
            );
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}