namespace Alpha.SecurityCode.HttpModules
{
    using System;
    using System.Web;
    using Configurations;
    using Services;
    using Web;
    using Sunplus.Agent.Model.Entities;

    public class SecurityCodeHttpModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(AcquireRequestState);
        }

        private void AcquireRequestState(object sender, EventArgs e)
        {
            if(HttpContext.Current == null
                || HttpContext.Current.Request == null
                || HttpContext.Current.Session == null)
            {
                return;
            }

            var currentRequest = HttpContext.Current.Request;
            var relativePath = currentRequest.Url.AbsolutePath.Replace(currentRequest.ApplicationPath, string.Empty);
            var isRequiredSecurityCode = SecurityCodeChecker.IsRequiredSecurityCode(relativePath);

            if (isRequiredSecurityCode)
            {
                var response = HttpContext.Current.Response;

                if (IsAjaxRequest(currentRequest))
                {
                    string requiredSecurityCodeJson = "{ \"code\"= 999991, \"message\": \"This action is required security code.\" }";
                    response.Clear();
                    response.ContentType = "application/json; charset=utf-8";
                    response.Write(requiredSecurityCodeJson);
                    response.End();
                }
                else
                {
                    response.ContentType = "text/html";
                    response.Redirect(SecurityCodeUrlBuilder.BuildVerifyingSecurityCodeUrl(HttpContext.Current.Request.Url.AbsoluteUri), true);
                }

            }
        }

        #endregion IHttpModule Members

        private static bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return (request["X-Requested-With"] == "XMLHttpRequest") 
                || (request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }
    }
}