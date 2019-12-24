using System.Web;
using Sunplus.Agent.Model.Entities;

namespace Alpha.ActivityTracking.Models
{
    public class AlphaUserSession : Fanex.ActivityTracking.Models.IUserSession
    {
        private const int AgentLevelId = 2;
        private const int MasterLevelId = 3;

        public int AdminId => UserSession.Current.AdminId;

        public string AdminName => UserSession.Current.AdminName;

        public int CustId => UserSession.Current.CustId;

        public string CustName => UserSession.Current.UserName;

        public string SessionID => HttpContext.Current.Session.SessionID;

        public string SiteId => UserSession.Current.Site;

        public byte LevelId => UserSession.Current.RoleId;

        public int OnUserId => UserSession.Current.OnUserId;

        public string Language => UserSession.Current.Language;

        public int SubAccId => UserSession.Current.SubAccId;

        public string SubUsername => UserSession.Current.SubUserName;

        public int CurrencyId => UserSession.Current.CurrencyId;

        public string CurrencyName => UserSession.Current.Currency;

        public string UplineIds => LevelId == AgentLevelId ?
            $"Master: { UserSession.Current.MRecommendId}, Super: { UserSession.Current.SRecommendId}"
            : LevelId == MasterLevelId ?
            $"Super: { UserSession.Current.SRecommendId}"
            : string.Empty;

        public bool RobotIsBrowserless => UserSession.Current.RobotIsBrowserless;

        public decimal RobotCaptchaScore => UserSession.Current.RobotCaptchaScore;

        public int RobotCaptchaFailedCount => UserSession.Current.RobotCaptchaFailedCount;
    }
}