namespace Alpha.Auth.Models
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using Configurations;
    using Web;

    public class RequestContext : IRequestContext
    {
        private string _url;
        private string _transId;
        private string rngArea = "CustomerSetting/Casino";

        public bool IsIgnore
        {
            get
            {
                var pattern = "([^\\s]+(\\.(?i)(jpg|png|gif|bmp|css|js))$)";

                Regex urlRegex = new Regex(pattern);

                return urlRegex.IsMatch(Url, (int)RegexOptions.IgnoreCase);
            }
        }

        public bool IsSessionNull
        {
            get
            {
                return HttpContext.Current.Session == null;
            }
        }

        public string Url
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_url))
                {
                    _url = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Substring(2);

                    if (IsRnGCasinoUri())
                    {
                        _url = GetRnGCasinoUri();
                        _url = _url.Remove(0, 1).ToLower();
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
                var request = HttpContext.Current.Request;
                return request["iswhcheck"] == "1";
            }
        }

        public int CustId
        {
            get
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

        private string GetRnGCasinoUri()
        {
            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"];

            var uri = string.Format("/{0}/{1}/{2}", rngArea, controller, action);

            return uri;
        }
    }
}
