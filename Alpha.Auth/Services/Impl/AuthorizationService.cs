namespace Alpha.Auth.Services.Impl
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Alpha.Auth.DataAccesses;
    using Alpha.Auth.DataAccesses.Impl;
    using Alpha.Auth.Services;
    using Configurations;
    using Entities;
    using Fanex.Common;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities;

    public class AuthorizationService : IAuthorizationService
    {
        private const string CustomerClosedStatusSessionKey = "Fanex.Razorbill.Security.Services.Authentication.CheckingClosedStatus";
        private const string SubAccountPermissionSessionKey = "Fanex.Razorbill.Security.Services.Authentication.CheckingCustomerPermission";

        private readonly IUserSession _currentUserSession;
        private readonly IAuthenticationDataAccess _dataAccess;
        private readonly IStatisticService _statisticService;

        private string _urlRequest;
        private object _blockStatus = new object();

        public AuthorizationService(string urlRequest)
        {
            _currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            _dataAccess = new AuthenticationDataAccess();
            _urlRequest = urlRequest;
            _statisticService = new StatisticService();
        }

        public string CustomerCurrentStatus
        {
            get
            {
                return (string)HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, CustomerClosedStatusSessionKey)];
            }

            set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, CustomerClosedStatusSessionKey)] = value;
            }
        }

        public string CustomerCurrentPermission
        {
            get
            {
                return (string)HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, SubAccountPermissionSessionKey)];
            }

            set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, SubAccountPermissionSessionKey)] = value;
            }
        }

        public bool CheckingClosedStatus()
        {
            lock (_blockStatus)
            {
                if (HttpContext.Current.Session == null)
                {
                    return true;
                }

                if (IgnoreSpecialFileTypes())
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(CustomerCurrentStatus))
                {
                    var sessionInfo = CustomerCurrentStatus.Split('_');
                    if (sessionInfo.Length > 1 &&
                        Converter.ToDateTime(sessionInfo[1]) >= DateTime.Now)
                    {
                        return false;
                    }
                }

                var isActive = _dataAccess.IsActive(_currentUserSession.CustId, _currentUserSession.SubAccId);
                if (!isActive)
                {
                    return true;
                }

                CustomerCurrentStatus =
                    string.Format("{0}_{1}", isActive, DateTime.Now.AddSeconds(Converter.ToInt(ConfigurationManager.AppSettings["CustomerStatusExpireTime"])));

                return false;
            }
        }

        public bool CheckBelonging(int custId, long transId, bool isDataWarehouse)
        {
            if (HttpContext.Current.Session == null)
            {
                return false;
            }

            var currentUrlAccess = GetUrlAccessByCustomerWithoutDirectoryPath();

            if (IgnoreSpecialFileTypes() ||
                currentUrlAccess.Equals("index.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var isBelonging = VerifyRequestingUrlAgainstConfigFile(custId, transId, isDataWarehouse, currentUrlAccess);

            return isBelonging;
        }

        public PermissionResult CheckingCustomerPermission(int custId, long transId, bool isDataWarehouse)
        {
            var customerPermission = new PermissionResult()
            {
                IsHasPermission = false,
                RedirectUrl = AuthorizationSection.Instance.IndexUrl
            };

            // session is null
            if (HttpContext.Current.Session == null)
            {
                customerPermission.RedirectUrl = AuthorizationSection.Instance.SignOutUrl;
                return customerPermission;
            }

            // ignore urls
            if (IgnoreSpecialFileTypes())
            {
                customerPermission.IsHasPermission = true;
                return customerPermission;
            }

            var authenticationSection = AuthorizationSection.Instance;
            var permission = authenticationSection.FullPermission;
            if (_currentUserSession.SubAccId > 0)
            {
                permission = GetSubPermission();
                if (permission.Equals("0"))
                {
                    return customerPermission;
                }
            }

            var newCustomerPermission = CheckingPageAccess(permission, authenticationSection, custId, transId, isDataWarehouse);
            customerPermission.IsHasPermission = newCustomerPermission.IsHasPermission;
            customerPermission.RedirectUrl = newCustomerPermission.RedirectUrl;

            return customerPermission;
        }

        private bool IgnoreSpecialFileTypes()
        {
            var pattern = "([^\\s]+(\\.(?i)(jpg|png|gif|bmp|css|js))$)";

            Regex urlRegex = new Regex(pattern);

            return urlRegex.IsMatch(_urlRequest, (int)RegexOptions.IgnoreCase);
        }

        private string GetUrlAccessByCustomerWithoutDirectoryPath()
        {
            var virtualDirectory = AuthorizationSection.Instance.VirtualDirectory;
            if (!string.IsNullOrWhiteSpace(virtualDirectory))
            {
                var regex = new Regex(string.Format("{0}/", virtualDirectory), RegexOptions.IgnoreCase);
                var urlWithOutVirtualDirectory = regex.Replace(_urlRequest, string.Empty);

                return urlWithOutVirtualDirectory;
            }

            return _urlRequest;
        }

        private bool VerifyRequestingUrlAgainstConfigFile(int custId, long transId, bool isDataWarehouse, string currentUrlAccess)
        {
            var isBelonging = true;
            var authenticationSection = AuthorizationSection.Instance;
            var permission = authenticationSection.FullPermission;

            // get all urls that allow he/she access by his/her level
            var levels = from q in authenticationSection.GroupLevel.Cast<GroupLevelElement>() where q.RoleId <= _currentUserSession.RoleId select q;
            if (levels.Any())
            {
                foreach (GroupLevelElement level in levels)
                {
                    // get setting of current url against config file
                    if (level != null && level.GroupLevel != null && level.GroupLevel.Count > 0)
                    {
                        var pageAccess = from q in level.GroupLevel.Cast<PageAccessElement>()
                                         where q.Url.Equals(currentUrlAccess, StringComparison.OrdinalIgnoreCase)
                                         select q;

                        if (pageAccess.Any())
                        {
                            foreach (PageAccessElement pageElement in pageAccess)
                            {
                                // Check belonging when:
                                // - downline id not equal 0
                                // - and current requesting url need to check belonging
                                // - and current requesting url not in special list: allow customers to update by themselves
                                if (custId != 0 && pageElement.IsCheckBelonging)
                                {
                                    if (!pageElement.IsAllowToDoByThemselves && UserSession.Current.CustId == custId)
                                    {
                                        return false;
                                    }

                                    if (pageElement.IsAllowToDoByThemselves && UserSession.Current.CustId == custId)
                                    {
                                        return true;
                                    }

                                    if (pageElement.IsViewedByGroupHeadline
                                        && _dataAccess.IsCustomerConsolidatedGroupBelonging(custId, _currentUserSession.CustId, _currentUserSession.SubAccId, transId, _currentUserSession.IsInternal))
                                    {
                                        return true;
                                    }

                                    return isDataWarehouse
                                        ? _dataAccess.IsBelongingOld(custId, _currentUserSession.CustId, transId)
                                        : _dataAccess.IsBelonging(custId, _currentUserSession.CustId, _currentUserSession.SubAccId, transId, _currentUserSession.IsInternal);
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return isBelonging;
        }

        private PermissionResult CheckingPageAccess(
            string customerPermission,
            AuthorizationSection authenticationSection,
            int custId,
            long transId,
            bool isDataWarehouse)
        {
            var permissionResult = new PermissionResult()
            {
                IsHasPermission = true
            };

            var levels = from q in authenticationSection.GroupLevel.Cast<GroupLevelElement>() where q.RoleId <= _currentUserSession.RoleId select q;
            if (levels.Any())
            {
                var urlWithOutVirtualDirectory = !string.IsNullOrWhiteSpace(AuthorizationSection.Instance.VirtualDirectory) ?
                    _urlRequest.ToUpper().Replace(string.Format("{0}/", AuthorizationSection.Instance.VirtualDirectory).ToUpper(), string.Empty) :
                    string.Empty;

                foreach (GroupLevelElement level in levels)
                {
                    if (level != null && level.GroupLevel != null && level.GroupLevel.Count > 0)
                    {
                        var pageAccess = from q in level.GroupLevel.Cast<PageAccessElement>()
                                         where q.Url.Equals(urlWithOutVirtualDirectory, StringComparison.OrdinalIgnoreCase)
                                         select q;

                        if (pageAccess.Any())
                        {
                            foreach (PageAccessElement pageElement in pageAccess)
                            {
                                var requiredPermission = pageElement.Permission.Split(',');
                                foreach (var item in requiredPermission)
                                {
                                    if (!customerPermission.Contains(item))
                                    {
                                        permissionResult.IsHasPermission = false;
                                        permissionResult.RedirectUrl = AuthorizationSection.Instance.AccessDeniedUrl;
                                        continue;
                                    }

                                    // Check belonging
                                    if (custId != 0 &&
                                        pageElement.IsCheckBelonging &&
                                        !(pageElement.IsAllowToDoByThemselves && UserSession.Current.CustId == custId))
                                    {
                                        var hasPermission = isDataWarehouse
                                            ? _dataAccess.IsBelongingOld(custId, _currentUserSession.CustId, transId)
                                            : _dataAccess.IsBelonging(custId, _currentUserSession.CustId, _currentUserSession.SubAccId, transId, _currentUserSession.IsInternal);
                                        if (!hasPermission)
                                        {
                                            permissionResult.IsHasPermission = false;
                                            permissionResult.RedirectUrl = AuthorizationSection.Instance.AccessDeniedUrl;
                                        }
                                    }

                                    if (!_currentUserSession.IsInternal && permissionResult.IsHasPermission)
                                    {
                                        _statisticService.UpdateFunctionUsage(pageElement.Url, pageElement.MainFunctionName, pageElement.SubFunctionName, _currentUserSession.Site);
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return permissionResult;
        }

        private string GetSubPermission()
        {
            if (!string.IsNullOrEmpty(CustomerCurrentPermission))
            {
                var sessionInfo = CustomerCurrentPermission.Split('_');
                if (sessionInfo.Length > 1 &&
                    Converter.ToDateTime(sessionInfo[1]) >= DateTime.Now)
                {
                    return Converter.ToString2(sessionInfo[0]);
                }
            }

            var newPermission = _dataAccess.GetSubAccountPermission(_currentUserSession.SubAccId);
            CustomerCurrentPermission =
                string.Format("{0}_{1}", newPermission, DateTime.Now.AddSeconds(Converter.ToInt(ConfigurationManager.AppSettings["CustomerStatusExpireTime"])));

            return newPermission;
        }
    }
}