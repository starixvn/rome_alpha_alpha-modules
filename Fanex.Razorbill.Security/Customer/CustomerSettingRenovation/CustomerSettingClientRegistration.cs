namespace Fanex.Razorbill.Security.Customer
{
    using System.Web;
    using Fanex.CustomerSetting.Client40;
    using Fanex.CustomerSetting.Client40.Models;
    //using Fanex.Razorbill.Module.Common.Helpers;
    using Sunplus.Agent.Model.BL;
    using Fanex.Razorbill.Security.Helpers;

    public class CustomerSettingClientRegistration : CustomerSettingRegistration
    {
        public override bool VerifyQueryPermission(PermissionContext permissionContext)
        {
            return true;
        }

        public override bool VerifyUpdatePermission(PermissionContext permissionContext)
        {  
            return this.IsAllowPermissionBasedOnSubAccAndInternalAcc();
        }

        public override bool VerifyMultipleUpdatePermission(PermissionContext permissionContext)
        {
            return this.IsAllowPermissionBasedOnSubAccAndInternalAcc();
        }

        public override SettingContext FetchSettingContext()
        {
            var currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            var settingContext = new SettingContext()
            {
                ActorId = (int)currentUserSession.UserId,
                ActorName = (string)currentUserSession.UserName,
                ActorLevel = (byte)currentUserSession.RoleId,
                SubAccountId = (int)currentUserSession.SubAccId,
                Language = (string)currentUserSession.Language,
                Domain = HttpContext.Current.Request.Url.Host,
                IP = IPHelper.CustomerIp()
            };
            
            return settingContext;
        }

        private bool IsAllowPermissionBasedOnSubAccAndInternalAcc()
        {
            var currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            bool subAccountPermission = !currentUserSession.IsSubAcc || currentUserSession.SubPageAccess.UpdateAccount;
            return !currentUserSession.IsInternal && subAccountPermission;
        }
    }
}