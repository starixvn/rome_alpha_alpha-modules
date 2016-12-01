namespace Sunplus.Agent.Model.BL.UserMessage
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Caching;
    using Fanex.Data;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.DL.UserMessage;

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

        public static string GetContentMsg(DataRow msg, string language)
        {
            string content = string.Empty;
            switch (language)
            {
                case "en-US":
                    content = msg["Content_en"] as string;
                    break;

                case "th-TH":
                    content = msg["Content_th"] as string;
                    break;

                case "zh-CN":
                    content = msg["Content_cs"] as string;
                    break;

                case "zh-TW":
                    content = msg["Content_ch"] as string;
                    if (string.IsNullOrEmpty(content))
                    {
                        content = msg["Content_cs"] as string;
                    }

                    break;

                case "ja-JP":
                    content = msg["Content_jp"] as string;
                    break;

                case "ko-KR":
                    content = msg["Content_ko"] as string;
                    break;

                case "vi-VN":
                    content = msg["Content_vn"] as string;
                    break;
            }

            if (string.IsNullOrEmpty(content))
            {
                content = msg["Content_en"] as string;
            }

            return content + string.Empty;
        }

        #region High-light and balloon messages

        /// <summary>
        /// Get top public and private messages. Index - 0:public, 1:private
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public DataTable GetMessages(int messageType, int siteId)
        {
            DataTable publicMessage = null;
            DataTable privateMessage = null;
            ICache cache = CacheService.CreateInstance(CacheType.MemoryCache); // Cache in 30 seconds
            string key = messageType.ToString() + "_" + siteId;
            publicMessage = cache.Get<DataTable>(key);
            if (publicMessage == null)
            {
                publicMessage = this.userMessageDAO.Acc_Msg_GetTopPubMessage(messageType, siteId, 0, false);
                cache.Set<DataTable>(key, publicMessage, DateTime.Now.AddSeconds(30));
            }

            privateMessage = this.userMessageDAO.Dot_Msg_GetTopPriMessage24(CurrentSession.CustId, 0, false);
            DataTable returnTable = UserMessageDAO.CreateDataTable();
            if (publicMessage.Rows.Count > 0)
            {
                UserMessageDAO.CopyDataTable(publicMessage, returnTable);
            }

            if (privateMessage.Rows.Count > 0)
            {
                UserMessageDAO.CopyDataTable(privateMessage, returnTable);
            }

            return returnTable;
        }

        /// <summary>
        /// Get count of unread private message by user
        /// </summary>
        /// <returns></returns>
        public int CountUnreadPriMessage()
        {
            return this.userMessageDAO.Dot_Msg_GetPriMsgCount(CurrentSession.CustId, CurrentSession.UserName);
        }

        /// <summary>
        /// Set unread private message is readed by user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        public bool SetReadPriMessage()
        {
            return this.userMessageDAO.Dot_Msg_SetPriMsgRead(CurrentSession.CustId, CurrentSession.UserName);
        }

        #endregion High-light and balloon messages

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

        #region Notification During Move-bet Process

        public string GetMoveBetsMessage(string language = null)
        {
            string message = string.Empty;

            var notification = GetNotificationDuringMoveBetsProcessFromCache();

            if (notification != null && notification.TryGetValue(language ?? CurrentSession.Language, out message))
            {
                return message;
            }

            return message;
        }

        public DateTime GetMoveBetsEndTime()
        {
            var endTime = DateTime.MinValue;
            var notification = GetNotificationDuringMoveBetsProcessFromCache();

            if (notification != null && notification.ContainsKey("endTime"))
            {
                DateTime.TryParse(notification["endTime"], out endTime);
            }

            return endTime;
        }

        private IDictionary<string, string> GetNotificationDuringMoveBetsProcessFromCache()
        {
            string key = "__Sunplus.Agent.Model.BL.UserMessage.UserMessageBO.NotificationMoveBets__";

            ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);
            IDictionary<string, string> notification = cache.Get<IDictionary<string, string>>(key);

            if (notification == null)
            {
                notification = GetNotificationDuringMoveBetsProcess();

                cache.Set(key, notification, DateTime.Now.AddMinutes(1));
            }

            return notification;
        }

        private IDictionary<string, string> GetNotificationDuringMoveBetsProcess()
        {
            IDictionary<string, string> moveBetsMessage = new Dictionary<string, string>(8);

            var dataTable = userMessageDAO.GetNotificationDuringMoveBetsProcess();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow drow = dataTable.Rows[0];
                object englishContent = drow["content_en"];

                if (englishContent != null && englishContent != DBNull.Value)
                {
                    moveBetsMessage["ja-JP"] = drow["content_jp"].ToString();
                    moveBetsMessage["ko-KR"] = drow["content_ko"].ToString();
                    moveBetsMessage["en-US"] = englishContent.ToString();
                    moveBetsMessage["th-TH"] = drow["content_th"].ToString();
                    moveBetsMessage["vi-VN"] = drow["content_vi"].ToString();
                    moveBetsMessage["zh-CN"] = drow["content_cs"].ToString();
                    moveBetsMessage["zh-TW"] = drow["content_ch"].ToString();
                    moveBetsMessage["endTime"] = drow["MBBUEndTime"].ToString();
                }
            }

            return moveBetsMessage;
        }

        #endregion Notification During Move-bet Process
    }
}