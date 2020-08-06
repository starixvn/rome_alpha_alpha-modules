namespace Sunplus.Agent.Model.BL.UserMessage
{
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.DL.UserMessage;
    using System;
    using System.Data;

    [Obsolete("Should rename to Sunplus.Agent.Model.Messages")]
    public class UserMessageBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        private bool limitConnection; // Default is true.
        private UserMessageDAO userMessageDAO;

        public UserMessageBO(string netSessionId)
            : this(netSessionId, true)
        {
        }

        public UserMessageBO(string netSessionId, bool isLimitConnection)
            : base(netSessionId)
        {
            this.limitConnection = isLimitConnection;
            this.userMessageDAO = new UserMessageDAO(this.limitConnection);
        }

        protected bool LimitConnection
        {
            get { return this.limitConnection; }
            set { this.limitConnection = value; }
        }      

        #region Position taking message

        /// <summary>
        /// Get status indicate whether PT should be displayed or not.
        /// </summary>
        /// <returns></returns>
        public bool GetPTMessageStatus()
        {
            int roleId = CurrentSession.RoleId;
            if (roleId != (int)UserTypes.Agent || CurrentSession.AdminId != 0)
            {
                return false;
            }

            UserMessageDAO dao = new UserMessageDAO();
            DataTable dt = dao.Age_MessageNoticeSelByCustID(CurrentSession.CustId);

            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            byte msgId = (byte)dt.Rows[0]["msgID"];
            return msgId == 1;
        }

        public DataTable GetPopupMessageStatus()
        {
            int roleId = CurrentSession.RoleId;
            if (roleId != (int)UserTypes.Agent || CurrentSession.AdminId != 0)
            {
                return null;
            }

            UserMessageDAO dao = new UserMessageDAO();
            DataTable dt = dao.Age_MessageNoticeSelByCustID(CurrentSession.CustId);
            return dt;
        }

        /// <summary>
        /// Update viewed status of PT message.
        /// </summary>
        /// <param name="isViewd"></param>
        public void UpdateViewedPTMessage(bool isViewed)
        {
            int roleId = CurrentSession.RoleId;
            if (roleId != (int)UserTypes.Agent || CurrentSession.AdminId != 0)
            {
                return;
            }

            UserMessageDAO dao = new UserMessageDAO();
            dao.Age_MessageNoticeUpdStatus(CurrentSession.CustId, isViewed);
        }

        /// <summary>
        /// Insert message into table Msg.
        /// </summary>
        public DataTable InsPersonalMessage(string content_en, string content_ch, string content_cs, string content_jp, string content_it, string content_th, string content_ko, string content_es, int msgType, bool isSticky, int matchId, bool isBatch)
        {
            int roleId = CurrentSession.RoleId;
            if (roleId != (int)UserTypes.Agent || CurrentSession.AdminId != 0)
            {
                return null;
            }

            UserMessageDAO dao = new UserMessageDAO();
            return dao.Dot_Msg_SetMessage(content_en, content_ch, content_cs, content_jp, content_it, content_th, content_ko, content_es, msgType, isSticky, CurrentSession.UserId, CurrentSession.UserName, AppSettings.LocalTest, matchId, isBatch);
        }

        /// <summary>
        /// Insert message into table MsgTarget.
        /// </summary>
        public void InsPersonalMsgTarget(decimal messageId, int targetId, string targetName, bool isRead, string betTransId)
        {
            int roleId = CurrentSession.RoleId;
            if (roleId != (int)UserTypes.Agent || CurrentSession.AdminId != 0)
            {
                return;
            }

            UserMessageDAO dao = new UserMessageDAO();
            dao.Dot_Msg_SetUsrMessage(messageId, targetId, targetName, isRead, betTransId);
        }

        public string MessageSelectOne(ref bool isSticky)
        {
            UserMessageDAO dao = new UserMessageDAO();

            DataTable dt = dao.Age_MessageSelOne(CurrentSession.RoleId);
            if (dt == null || dt.Rows.Count == 0)
            {
                return string.Empty;
            }

            string message = (CurrentSession.Language == "en-US" ? dt.Rows[0]["message_en"] : dt.Rows[0]["message_cn"]) as string;

            isSticky = (bool)dt.Rows[0]["IsSticky"];

            return message;
        }

        #endregion Position taking message

        #region Announcement

        public DataTable GetMessages(int targetID, string targetID2, bool isTest)
        {
            return this.userMessageDAO.Age_CustomerSelMessages(targetID, targetID2, isTest);
        }

        public DataTable GetMessages(int targetID, string targetID2, DateTime lastPostTime, int totalMessage, bool isTest)
        {
            return this.userMessageDAO.Age_CustomerSelMessages(targetID, targetID2, lastPostTime, totalMessage, isTest);
        }

        public void SetReadPriMsg(int userID, string msgIds)
        {
            this.userMessageDAO.Age_CustomerUpdMessagesRead(userID, msgIds);
        }

        private void InitiateUserMessage()
        {
            if (this.userMessageDAO == null)
            {
                this.userMessageDAO = new UserMessageDAO(this.limitConnection);
            }
        }

        #endregion Announcement
    }
}