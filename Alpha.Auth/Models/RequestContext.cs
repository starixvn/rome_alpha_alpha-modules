namespace Alpha.Auth.Models
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Configurations;
    using Fanex.Common;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities;
    using Web;

    public class RequestContext : IRequestContext
    {
        private const string RNGCasinoArea = "CustomerSetting/Casino";
        private string _url;
        private int _custId = 0;

        public string Url
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_url))
                {
                    _url = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Substring(2);

                    if (IsRNGCasinoUri())
                    {
                        return GetRnGCasinoUri().Remove(0, 1).ToLower();
                    }
                }

                return _url;
            }
        }

        public long TransId
        {
            get
            {
                var request = HttpContext.Current.Request;
                long transId;

                long.TryParse(request["transid"], out transId);

                if (transId == 0)
                {
                    long.TryParse(request["refno"], out transId);
                }

                return transId;
            }
        }

        public bool IsDataStoreInDataWarehouse
        {
            get
            {
                return HttpContext.Current.Request["iswhcheck"] == "1";
            }
        }

        public int CustId
        {
            get
            {
                if (_custId > 0)
                {
                    return _custId;
                }

                var custIdQueryStringParamNames = AuthorizationSection.Instance.CustIdQueryStringParamNames;
                var custIdJsonParamNames = AuthorizationSection.Instance.CustIdJsonParamNames;
                var request = HttpContext.Current.Request;

                foreach (string custIdQueryStringParamName in custIdQueryStringParamNames)
                {
                    if (string.IsNullOrWhiteSpace(custIdQueryStringParamName))
                    {
                        continue;
                    }

                    var currentParamName = Regex.Replace(custIdQueryStringParamName, @"^\s+", string.Empty);
                    _custId = Converter.ToInt(request[currentParamName]);

                    if (_custId > 0)
                    {
                        break;
                    }

                    if (request.Form.Count > 0
                        && request.Form.AllKeys.Contains(currentParamName))
                    {
                        _custId = Converter.ToInt(request.Form[currentParamName]);

                        if (_custId > 0)
                        {
                            break;
                        }
                    }
                }

                if (_custId == 0)
                {
                    foreach (string custIdJsonParamName in custIdJsonParamNames)
                    {
                        if (_custId == 0 &&
                            !string.IsNullOrEmpty(request[custIdJsonParamName]))
                        {
                            _custId = RequestAnalyst.GetCustomerId(request[custIdJsonParamName]);
                        }

                        if (_custId > 0)
                        {
                            break;
                        }
                    }
                }

                return _custId;
            }
        }

        public IUserSession UserSession
        {
            get
            {
               return UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            }
        }

        private bool IsRNGCasinoUri()
        {
            var url = HttpContext.Current.Request.Path.ToString();

            return url.IndexOf(RNGCasinoArea, StringComparison.OrdinalIgnoreCase) != -1;
        }

        private string GetRnGCasinoUri()
        {
            var routeData = HttpContext.Current.Request.RequestContext.RouteData;
            var controller = routeData.Values["controller"];
            var action = routeData.Values["action"];

            return string.Format("/{0}/{1}/{2}", RNGCasinoArea, controller, action);
        }
    }
}