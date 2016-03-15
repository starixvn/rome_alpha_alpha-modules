namespace Alpha.Auth.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Auth.DataAccesses;
    using Alpha.Auth.DataAccesses.Impl;
    using Alpha.Auth.Services;
    using Configurations;
    using Models;

    /// <summary>
    /// Permission Description
    /// [A]: Create
    /// [B]: Update
    /// [C]: Transfer
    /// [D]: Total Bets
    /// [E]: Reports
    /// [F]: Forcast
    /// [H]: Member Information
    /// [G]: View Log
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private const string IsCustomerActiveCacheKey = "Alpha.Auth.CustContext.IsCustomerActiveCacheKey_{0}_{1}";
        private const string SubAccountPermissionCacheKey = "Alpha.Auth.CustContext.SubAccountPermissionCacheKey_{0}_{1}";
        private readonly int _cachedExpireTime;

        private readonly IAuthenticationDataAccess _authDataAccess;
        private readonly IStatisticService _statisticService;
        private readonly IRequestContext _requestContext;
        private readonly ICachingService _cachingService;

        public AuthorizationService(
            IStatisticService statisticService = null,
            IRequestContext requestContext = null,
            ICachingService cachingService = null,
            IAuthenticationDataAccess dataAccess = null)
        {
            _authDataAccess = dataAccess ?? new AuthenticationDataAccess();
            _statisticService = statisticService ?? new StatisticService();
            _cachingService = cachingService ?? new CachingService();
            _requestContext = requestContext ?? new RequestContext();
            _cachedExpireTime = AuthorizationSection.Instance.CachedExpireTime;
        }

        public bool IsAccountActive()
        {
            var isActive = _cachingService.GetOrAdd(
                string.Format(IsCustomerActiveCacheKey, _requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId),
                () =>
                {
                    return _authDataAccess.IsActive(_requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId);
                },
                _cachedExpireTime);

            return isActive;
        }

        public bool CheckBelonging()
        {
            var isBelonging = true;
            var authenticationSection = AuthorizationSection.Instance;

            var groupPageAccesses = authenticationSection.GroupPageAccesses.Cast<GroupPageAccessElement>()
                .Where(group => group.RoleId <= _requestContext.UserSession.RoleId);

            foreach (GroupPageAccessElement groupPageAccess in groupPageAccesses)
            {
                var pageAccesses = groupPageAccess.PageAccesses.Cast<PageAccessElement>()
                    .Where(q => q.Url.Equals(_requestContext.Url, StringComparison.OrdinalIgnoreCase));

                foreach (PageAccessElement pageAccess in pageAccesses)
                {
                    if (_requestContext.CustId != 0
                        && pageAccess.IsCheckBelonging)
                    {
                        return CheckBelongingOnPage(pageAccess);
                    }
                }
            }

            return isBelonging;
        }

        private bool CheckBelongingOnPage(PageAccessElement pageAccess)
        {
            // Check belonging when:
            // - downline id not equal 0
            // - and current requesting url need to check belonging
            // - and current requesting url not in special list: allow customers to update by themselves
            if (_requestContext.UserSession.CustId == _requestContext.CustId)
            {
                return pageAccess.IsAllowToDoByThemselves;
            }

            if (pageAccess.IsViewedByGroupHeadline
                && _authDataAccess.IsCustomerConsolidatedGroupBelonging(
                    _requestContext.CustId,
                    _requestContext.UserSession.CustId,
                    _requestContext.UserSession.SubAccId,
                    _requestContext.TransId,
                    _requestContext.UserSession.IsInternal))
            {
                return true;
            }

            return _requestContext.IsDataStoreInDataWarehouse

                ? _authDataAccess.IsBelongingOld(
                    _requestContext.CustId,
                    _requestContext.UserSession.CustId,
                    _requestContext.TransId)

                : _authDataAccess.IsBelonging(
                    _requestContext.CustId,
                    _requestContext.UserSession.CustId,
                    _requestContext.UserSession.SubAccId,
                    _requestContext.TransId,
                    _requestContext.UserSession.IsInternal);
        }

        public bool CheckPermission(string url = null)
        {
            url = url ?? _requestContext.Url;

            var authenticationSection = AuthorizationSection.Instance;
            var permission = authenticationSection.FullPermission;

            if (_requestContext.UserSession.SubAccId > 0)
            {
                permission = GetSubPermission();

                if (permission.Equals("0"))
                {
                    return true;
                }
            }

            var groupPageAccesses = authenticationSection.GroupPageAccesses.Cast<GroupPageAccessElement>()
                .Where(group => group.RoleId <= _requestContext.UserSession.RoleId);

            return CheckPermissionOnGroupOfPage(
                permission, 
                groupPageAccesses,
                url,
                _requestContext.CustId, 
                _requestContext.TransId);
        }

        private bool CheckPermissionOnGroupOfPage(
            string customerPermission,
            IEnumerable<GroupPageAccessElement> groupPageAccesses,
            string url,
            int custId,
            long transId)
        {
            foreach (var groupPageAccess in groupPageAccesses)
            {
                var pageAccesses = groupPageAccess.PageAccesses.Cast<PageAccessElement>()
                    .Where(pageAccess => pageAccess.Url.Equals(url, StringComparison.OrdinalIgnoreCase));

                foreach (var pageAccess in pageAccesses)
                {
                    var hasPermissionOnPage = CheckPermissionOnPage(customerPermission, custId, transId, pageAccess);

                    if (!hasPermissionOnPage)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckPermissionOnPage(string customerPermission, int requestCustId, long transId, PageAccessElement pageElement)
        {
            var permissions = pageElement.Permission.Split(',');
            var hasPermisison = true;

            foreach (var permission in permissions)
            {
                if (!customerPermission.Contains(permission))
                {
                    return false;
                }

                if (requestCustId != 0
                    && pageElement.IsCheckBelonging
                    && pageElement.IsAllowToDoByThemselves
                    && _requestContext.UserSession.CustId != requestCustId)
                {
                    hasPermisison = false;
                }

                if (!_requestContext.UserSession.IsInternal && hasPermisison)
                {
                    //// Update page visit if user has permission and is not internal user.
                    _statisticService.UpdateFunctionUsage(
                        pageElement.Url,
                        pageElement.MainFunctionName,
                        pageElement.SubFunctionName,
                        _requestContext.UserSession.Site);
                }
            }

            return hasPermisison;
        }

        private string GetSubPermission()
        {
            var subAccountPermission = _cachingService.GetOrAdd(
                string.Format(SubAccountPermissionCacheKey, _requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId),
                () =>
                {
                    return _authDataAccess.GetSubAccountPermission(_requestContext.UserSession.SubAccId);
                },
                _cachedExpireTime);

            return subAccountPermission;
        }
    }
}