namespace Alpha.Auth.Models
{
    using System;
    using System.Web;
    using Configurations;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities;
    using Web;

    public class RequestContext : IRequestContext
    {
        private readonly IRequestAnalyst RequestAnalyst;
        private string _url;
        private int _custId = 0;
        private string _custIds = string.Empty;

        public RequestContext() : this(null)
        {

        }

        public RequestContext(IRequestAnalyst requestAnalyst)
        {
            this.RequestAnalyst = requestAnalyst ?? new RequestAnalyst();
        }

        public string Url
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_url))
                {
                    _url = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Substring(2);
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

                    if (!string.IsNullOrEmpty(request[custIdQueryStringParamName]))
                    {
                        _custId = Convert.ToInt32(request[custIdQueryStringParamName]);
                        if (_custId > 0)
                        {
                            break;
                        }

                        _custId = Convert.ToInt32(request.Form[custIdQueryStringParamName]);
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
                        _custId = RequestAnalyst.GetCustomerId(custIdJsonParamName);

                        if (_custId > 0)
                        {
                            break;
                        }
                    }
                }

                return _custId;
            }
        }

        public string CustIds
        {
            get
            {
                if (!string.IsNullOrEmpty(_custIds))
                {
                    return _custIds;
                }

                var custIdsQueryStringParamNames = AuthorizationSection.Instance.CustIdsQueryStringParamNames;
                var request = HttpContext.Current.Request;

                foreach (string custIdQueryStringParamName in custIdsQueryStringParamNames)
                {
                    if (string.IsNullOrWhiteSpace(custIdQueryStringParamName))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(request[custIdQueryStringParamName]))
                    {
                        _custIds = request[custIdQueryStringParamName];
                        if (!string.IsNullOrEmpty(_custIds))
                        {
                            _custIds = _custIds.Replace(',', '^');
                            break;
                        }

                        _custIds = request.Form[custIdQueryStringParamName];
                        if (!string.IsNullOrEmpty(_custIds))
                        {
                            _custIds = _custIds.Replace(',', '^');
                            break;
                        }
                    }
                }

                return _custIds;
            }
        }
        public IUserSession UserSession
        {
            get
            {
                return UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            }
        }  
    }
}