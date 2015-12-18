namespace Fanex.Razorbill.Security.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Fanex.Razorbill.Security.Sessions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BasedController : Controller
    {
        public BasedController()
        {
            SecurityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
        }

        protected ISecurityCodeSession SecurityCodeSession { get; private set; }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }

        /// <summary>
        /// Invokes the action in the current controller context.
        /// </summary>
        protected override void ExecuteCore()
        {
            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(SecurityCodeSession.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            base.ExecuteCore();
        }
    }
}