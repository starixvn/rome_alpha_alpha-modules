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
    using Models;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities;

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthenticationDataAccess _authDataAccess;
        private readonly IStatisticService _statisticService;
        private readonly ICustContext _custContext;
        private readonly IRequestContext _requestContext;

        private object _blockStatus = new object();

        public AuthorizationService(
            IStatisticService statisticService = null,
            ICustContext custContext = null,
            IRequestContext requestContext = null)
        {
            _authDataAccess = new AuthenticationDataAccess();
            _statisticService = statisticService ?? new StatisticService();
            _custContext = custContext ?? new CustContext();
            _requestContext = requestContext ?? new RequestContext();
        }

        public bool CheckClosedStatus()
        {
            lock (_blockStatus)
            {
                if (_requestContext.IsSessionNull)
                {
                    return true;
                }

                if (_requestContext.IsIgnore
                    || _custContext.StatusExpiredTime >= DateTime.Now)
                {
                    return false;
                }

                var isActive = _authDataAccess.IsActive(_custContext.UserSession.CustId, _custContext.UserSession.SubAccId);
                if (!isActive)
                {
                    return true;
                }

                _custContext.IsStatusActive = isActive;

                return false;
            }
        }

        public bool CheckBelonging()
        {
            if (_requestContext.IsSessionNull)
            {
                return false;
            }

            if (_requestContext.IsIgnore ||
                _requestContext.Url.Equals("index.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return VerifyRequestingUrlAgainstConfigFile(_requestContext.CustId, _requestContext.TransId, _requestContext.Url);
        }

        public PermissionResult CheckPermission()
        {
            var customerPermission = new PermissionResult();

            // session is null
            if (_requestContext.IsSessionNull)
            {
                customerPermission.RedirectUrl = AuthorizationSection.Instance.SignOutUrl;

                return customerPermission;
            }

            // ignore urls
            if (_requestContext.IsIgnore)
            {
                customerPermission.HasPermission = true;

                return customerPermission;
            }

            var authenticationSection = AuthorizationSection.Instance;
            var permission = authenticationSection.FullPermission;
            if (_custContext.UserSession.SubAccId > 0)
            {
                permission = GetSubPermission();

                if (permission.Equals("0"))
                {
                    return customerPermission;
                }
            }

            var newCustomerPermission = CheckPageAccess(permission, authenticationSection, _requestContext.CustId, _requestContext.TransId);

            customerPermission.HasPermission = newCustomerPermission.HasPermission;
            customerPermission.RedirectUrl = newCustomerPermission.RedirectUrl;

            return customerPermission;
        }

        private bool VerifyRequestingUrlAgainstConfigFile(int custId, long transId, string currentUrlAccess)
        {
            var isBelonging = true;
            var authenticationSection = AuthorizationSection.Instance;
            var permission = authenticationSection.FullPermission;

            // get all urls that allow he/she access by his/her level
            
            var levels = authenticationSection.GroupLevel.Cast<GroupLevelElement>().Where(groupLevel => groupLevel.RoleId <= _custContext.UserSession.RoleId);
            if (levels.Any())
            {
                foreach (GroupLevelElement level in levels)
                {
                    // get setting of current url against config file
                    if (level != null && level.GroupLevel != null && level.GroupLevel.Count > 0)
                    {
                        var pageAccess = level.GroupLevel.Cast<PageAccessElement>().Where(q => q.Url.Equals(currentUrlAccess, StringComparison.OrdinalIgnoreCase));

                        if (pageAccess.Any())
                        {
                            foreach (PageAccessElement pageElement in pageAccess)
                            {
                                // Check belonging when:
                                // - downline id not equal 0
                                // - and current requesting url need to check belonging
                                // - and current requesting url not in special list: allow customers to update by themselves
                                if (custId != 0 
                                    && pageElement.IsCheckBelonging)
                                {
                                    if (UserSession.Current.CustId == custId)
                                    {
                                        return pageElement.IsAllowToDoByThemselves;
                                    }

                                    if (pageElement.IsViewedByGroupHeadline
                                        && _authDataAccess.IsCustomerConsolidatedGroupBelonging(
                                            custId, 
                                            _custContext.UserSession.CustId, 
                                            _custContext.UserSession.SubAccId, 
                                            transId, 
                                            _custContext.UserSession.IsInternal))
                                    {
                                        return true;
                                    }

                                    return _requestContext.IsDataStoreInDataWarehouse
                                        ? _authDataAccess.IsBelongingOld(custId, _custContext.UserSession.CustId, transId)
                                        : _authDataAccess.IsBelonging(custId, _custContext.UserSession.CustId, _custContext.UserSession.SubAccId, transId, _custContext.UserSession.IsInternal);
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return isBelonging;
        }

        private PermissionResult CheckPageAccess(
            string customerPermission,
            AuthorizationSection authenticationSection,
            int custId,
            long transId)
        {
            var permissionResult = new PermissionResult()
            {
                HasPermission = true
            };

            var levels = authenticationSection.GroupLevel.Cast<GroupLevelElement>().Where(q => q.RoleId <= _custContext.UserSession.RoleId);
            if (levels.Any())
            {
                foreach (GroupLevelElement level in levels)
                {
                    if (level != null && level.GroupLevel != null && level.GroupLevel.Count > 0)
                    {
                        var pageAccess = level.GroupLevel.Cast<PageAccessElement>().Where(q => q.Url.Equals(_requestContext.Url, StringComparison.OrdinalIgnoreCase));
                        if (pageAccess.Any())
                        {
                            foreach (PageAccessElement pageElement in pageAccess)
                            {
                                var requiredPermission = pageElement.Permission.Split(',');
                                foreach (var item in requiredPermission)
                                {
                                    if (!customerPermission.Contains(item))
                                    {
                                        permissionResult.HasPermission = false;
                                        permissionResult.RedirectUrl = AuthorizationSection.Instance.AccessDeniedUrl;

                                        continue;
                                    }

                                    // Check belonging
                                    if (custId != 0 &&
                                        pageElement.IsCheckBelonging &&
                                        !(pageElement.IsAllowToDoByThemselves && UserSession.Current.CustId == custId))
                                    {
                                        var hasPermission = _requestContext.IsDataStoreInDataWarehouse
                                            ? _authDataAccess.IsBelongingOld(custId, _custContext.UserSession.CustId, transId)
                                            : _authDataAccess.IsBelonging(custId, _custContext.UserSession.CustId, _custContext.UserSession.SubAccId, transId, _custContext.UserSession.IsInternal);

                                        if (!hasPermission)
                                        {
                                            permissionResult.HasPermission = false;
                                            permissionResult.RedirectUrl = AuthorizationSection.Instance.AccessDeniedUrl;
                                        }
                                    }

                                    if (!_custContext.UserSession.IsInternal 
                                        && permissionResult.HasPermission)
                                    {
                                        _statisticService.UpdateFunctionUsage(pageElement.Url, pageElement.MainFunctionName, pageElement.SubFunctionName, _custContext.UserSession.Site);
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
            if (_custContext.PermissionExpiredTime >= DateTime.Now)
            {
                return _custContext.Permission;
            }

            var newPermission = _authDataAccess.GetSubAccountPermission(_custContext.UserSession.SubAccId);

            _custContext.Permission = newPermission;

            return newPermission;
        }
    }
}