namespace Fanex.Razorbill.Module.Company.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// Contact Us controller.
    /// </summary>
    public class ContactUsController : Controller
    {
        /// <summary>
        /// Disable asynchronous support to provide backward compatibility.
        /// </summary>
        /// <returns>True if asynchronous support is disabled; otherwise false.</returns>
        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }

        public ActionResult Index()
        {
            return View("~/Areas/Company/Views/ContactUs/Index.cshtml");
        }

        /// <summary>
        /// Invokes the action in the current controller context.
        /// </summary>
        protected override void ExecuteCore()
        {
            string cultureName = UserSession.Current.Language;
            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = "en-US"; // default
                UserSession.Current.Language = cultureName;
            }

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            System.Globalization.CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

            base.ExecuteCore();
        }
    }
}
