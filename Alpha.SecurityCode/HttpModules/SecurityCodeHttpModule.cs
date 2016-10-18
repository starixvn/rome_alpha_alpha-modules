namespace Alpha.SecurityCode.HttpModules
{
    using System;
    using System.Web;
    using Services;
    using Web;

    public class SecurityCodeHttpModule : IHttpModule
    {
        private readonly SecurityCodeChecker securityCodeChecker;
        private readonly SecurityCodeHttpContext securityCodeHttpContext;

        public SecurityCodeHttpModule() : this(null, null)
        {
        }

        internal SecurityCodeHttpModule(SecurityCodeChecker securityCodeChecker, SecurityCodeHttpContext securityCodeHttpContext)
        {
            this.securityCodeChecker = securityCodeChecker ?? new SecurityCodeCheckerImpl();
            this.securityCodeHttpContext = securityCodeHttpContext ?? new SecurityCodeHttpContextImpl();
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                return;
            }

            context.AcquireRequestState += new EventHandler(AcquireRequestState);
        }

        internal void AcquireRequestState(object sender, EventArgs e)
        {
            if (!securityCodeHttpContext.IsValidRequest())
            {
                return;
            }

            var relativePath = securityCodeHttpContext.GetRelativePath();
            var isRequiredSecurityCode = this.securityCodeChecker.IsRequiredSecurityCode(relativePath);

            if (isRequiredSecurityCode)
            {
                if (securityCodeHttpContext.IsAjaxRequest())
                {
                    securityCodeHttpContext.DenyAjaxRequest();
                }
                else
                {
                    securityCodeHttpContext.RedirectToVerifySecurityCodePage();
                }
            }
        }
    }
}