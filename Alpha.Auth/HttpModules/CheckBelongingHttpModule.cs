namespace Alpha.Auth.HttpModules
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;
    using Configurations;
    using Services;
    using Services.Impl;
    using Sunplus.Agent.Model.Entities;
    using Web;

    public class CheckBelongingHttpModule : IHttpModule
    {
        private string rngArea = "CustomerSetting/Casino";

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
            int custId = 0;
            long transId = 0;
            int masterId = 0;

            bool isDataWarehouse;
            string url;
            GetRequestParams(ref custId, ref transId, ref masterId, out isDataWarehouse, out url);

            if (string.IsNullOrEmpty(url) ||
                url.Replace("/", string.Empty).Equals(AuthorizationSection.Instance.VirtualDirectory, StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Current.Response.Redirect(FormsAuthentication.DefaultUrl, true);
                return;
            }

            IAuthorizationService authorizationService = new AuthorizationService(url);
            if (UserSession.Current.CustId != 0 
                && !string.IsNullOrWhiteSpace(url) 
                && !UserSession.Current.IsInternal)
            {
                if (authorizationService.CheckingClosedStatus())
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
                    return;
                }
            }

            var isBelonging = authorizationService.CheckBelonging(custId, transId, isDataWarehouse);
            if (!isBelonging)
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
                return;
            }
        }

        private void GetRequestParams(ref int custId, ref long transId, ref int masterId, out bool isDataWarehouse, out string url)
        {
            var request = HttpContext.Current.Request;
            long.TryParse(request["transid"], out transId);
            if (transId == 0)
            {
                long.TryParse(request["refno"], out transId);
            }

            int.TryParse(request["idmaster"], out masterId);
            isDataWarehouse = request["iswhcheck"] == "1";

            url = GetRequestPath();

            custId = GetCustomerIdFromRequest();
        }

        private bool IsRnGCasinoUri()
        {
            var url = HttpContext.Current.Request.Path.ToString();
            if (url.IndexOf(rngArea, StringComparison.OrdinalIgnoreCase) != -1)
            {
                return true;
            }

            return false;
        }

        private string GetRequestPath()
        {
            var url = HttpContext.Current.Request.Path;
            if (IsRnGCasinoUri())
            {
                url = GetRnGCasinoUri();
            }

            url = url.Remove(0, 1).ToLower();

            return url;
        }

        private string GetRnGCasinoUri()
        {
            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"];

            var uri = string.Format("/{0}/{1}/{2}", rngArea, controller, action);

            return uri;
        }

        private int GetCustomerIdFromRequest()
        {
            var belongingParams = AuthorizationSection.Instance.BelongingParams;
            var paramArray = belongingParams.Split(',');
            var custId = 0;
            var request = HttpContext.Current.Request;
            foreach (string paramRequest in paramArray)
            {
                if (string.IsNullOrWhiteSpace(paramRequest))
                {
                    continue;
                }

                var currentParamName = Regex.Replace(paramRequest, @"^\s+", string.Empty);
                custId = Fanex.Common.Converter.ToInt(request[currentParamName]);
                if (custId > 0)
                {
                    break;
                }
            }

            if (custId == 0 &&
                !string.IsNullOrEmpty(request["txtCustomerObject"]))
            {
                custId = RequestAnalyst.GetCustomerId(request["txtCustomerObject"]);
            }

            if (custId == 0 &&
                !string.IsNullOrEmpty(request["newCustomer"]))
            {
                custId = RequestAnalyst.GetCustomerId(request["newCustomer"]);
            }

            return custId;
        }

        #endregion IHttpModule Members
    }
}