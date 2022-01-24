namespace Alpha.SecurityCode.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Alpha.SecurityCode.Configurations;
    using Services;

    public class SecurityCodeHttpContextImpl : SecurityCodeHttpContext
    {
        private readonly SecurityCodePathBuilder securityCodePathBuilder;

        public SecurityCodeHttpContextImpl() : this(null)
        {
        }

        internal SecurityCodeHttpContextImpl(SecurityCodePathBuilder securityCodePathBuilder)
        {
            this.securityCodePathBuilder = securityCodePathBuilder ?? new SecurityCodePathBuilderImpl();
        }

        public void DenyAjaxRequest()
        {
            var response = HttpContext.Current.Response;
            string requiredSecurityCodeJson = "{ \"code\": 999991, \"message\": \"This action is required security code.\" }";
            response.Clear();
            response.ContentType = "application/json; charset=utf-8";
            response.Write(requiredSecurityCodeJson);
            response.End();
        }

        public string GetRelativePath()
        {
            var request = HttpContext.Current.Request;
            return request.Url.PathAndQuery.Replace(request.ApplicationPath, string.Empty);
        }

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Reviewed")]
        public bool IsAjaxRequest()
        {
            var request = HttpContext.Current.Request;
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return (request["X-Requested-With"] == "XMLHttpRequest")
                || (request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest")
                || (request.Headers != null && request.Headers["IsPostAjax"] == "true");
        }

        public bool IsValidRequest()
        {
            if (HttpContext.Current == null
                  || HttpContext.Current.Request == null
                  || HttpContext.Current.Session == null)
            {
                return false;
            }

            return true;
        }

        public void RedirectToVerifySecurityCodePage()
        {
            var httpContext = HttpContext.Current;
            var request = httpContext.Request;
            var requestHeaders = request.Headers;
            var response = httpContext.Response;

            if (requestHeaders != null && requestHeaders["Content-type"] == "application/x-www-form-urlencoded")
            {
                response.Redirect(this.securityCodePathBuilder.BuildVerifyingSecurityCodePath(), true);
            }
            else
            {
                var rawUrl = request.RawUrl;

                if (httpContext.Session.IsCookieless)
                {
                    // Modify Url with session id in it
                    rawUrl = response.ApplyAppPathModifier(rawUrl);

                    if (AppSettings.SecurityCodeConfig.IsStandaloneSite)
                    {
                        rawUrl = $"https://{requestHeaders["Host"]}{rawUrl}";
                    }
                }

                response.Redirect(this.securityCodePathBuilder.BuildVerifyingSecurityCodePath(rawUrl), true);
            }
        }
    }
}