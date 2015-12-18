namespace Sunplus.Agent.Model.BL.Base
{
    using System;
    using Sunplus.Agent.Model.Entities;

    [Obsolete("Should rename to Sunplus.Agent.Model.Base")]
    public class BOBase
    {
        private readonly IUserSession _userSession;
        private readonly string _netSessionId;

        public BOBase(string netSessionId)
        {
            this._netSessionId = netSessionId;
            this._userSession = UserSessionService.GetUserSessionInstance(netSessionId);
        }

        public IUserSession CurrentSession
        {
            get
            {
                return this._userSession;
            }
        }

        public string NetSessionId
        {
            get
            {
                return this._netSessionId;
            }
        }
    }
}