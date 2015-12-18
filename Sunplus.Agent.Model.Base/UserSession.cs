namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Web;
    using Sunplus.Agent.Model.BL;

    [Serializable]
    [Obsolete("Should rename to Sunplus.Agent.Model.Base")]
    public class UserSession : UserSessionBase
    {
        public UserSession(string netSessionId)
            : base(netSessionId)
        {
        }

        public UserSession(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public static IUserSession Current
        {
            get
            {
                return UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            }
        }
    }
}