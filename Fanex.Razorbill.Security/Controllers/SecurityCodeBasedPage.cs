namespace Fanex.Razorbill.Security.Controllers
{
    using System;
    using System.Web.UI;
    using Fanex.Razorbill.Security.Services;

    /// <summary>
    /// SecurityCodeBasedPage class.
    /// </summary>
    public class SecurityCodeBasedPage : Page
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string redirectUrl = SecurityCodeChecker.GetRedirectUrl();
                if (redirectUrl != null)
                {
                    Response.Redirect(redirectUrl, true);
                }
            }

            base.OnLoad(e);
        }
    }
}
