namespace Sunplus.Agent.Model.DL.UserMessage
{
    using System;
    using System.Collections;
    using System.Data;

    [Obsolete("Should rename to Sunplus.Agent.Model.Messages")]
    public class UserMessageDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        private static int maxWaitingConnection = 2;
        private static object lockObject = new object();
        private static volatile int waitingConnection = 0;

        private static string[] colNameOfDataTable = new string[]
            {
                "Content_ch", "System.String",
                "Content_cs", "System.String",
                "Content_en", "System.String",
                "Content_it", "System.String",
                "Content_jp", "System.String",
                "Content_ko", "System.String",
                "Content_th", "System.String",
                "Content_vn", "System.String",
                "CreateTime", "System.DateTime",
                "CreatorId", "System.Int32",
                "CreatorName", "System.String",
                "IsSticky", "System.Boolean",
                "LastUpdateName", "System.String",
                "LastUpdateTime", "System.DateTime",
                "LastUpdator", "System.Int32",
                "MessageId", "System.Int32",
                "MessageType", "System.Int32",
                "Poster", "System.String",
                "PostTime", "System.DateTime",
                "UserId", "System.Int32",
                "UserName", "System.String"
            };

        private bool limitConnection; // Default is true.

        public UserMessageDAO()
            : this(true)
        {
        }

        public UserMessageDAO(bool isLimitConnection)
        {
            this.limitConnection = isLimitConnection;
        }

        public static int MaxWaitingConnection
        {
            get { return UserMessageDAO.maxWaitingConnection; }
            set { UserMessageDAO.maxWaitingConnection = value; }
        }

        protected bool LimitConnection
        {
            get { return this.limitConnection; }
            set { this.limitConnection = value; }
        }

        #region Helper

        public static DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;

            for (int idxElement = 0; idxElement < colNameOfDataTable.Length; idxElement += 2)
            {
                dc = new DataColumn();
                dc.ColumnName = colNameOfDataTable[idxElement];
                dc.DataType = Type.GetType(colNameOfDataTable[idxElement + 1]);
                dt.Columns.Add(dc);
            }

            return dt;
        }

        public static void CopyDataTable(DataTable srcTable, DataTable desTable)
        {
            foreach (DataRow row in srcTable.Rows)
            {
                AddDataRow(row, desTable);
            }
        }

        #endregion Helper

        #region High-light and balloon messages

        public new DataTable ExecSpTable(string name, Hashtable parameters, int timeOut)
        {
            DataTable result = null;

            if (this.limitConnection)
            {
                if (waitingConnection > maxWaitingConnection)
                {
                    return null;
                }

                lock (lockObject)
                {
                    waitingConnection++;
                }

                try
                {
                    result = base.ExecSpTable(name, parameters, timeOut);
                }
                finally
                {
                    lock (lockObject)
                    {
                        waitingConnection--;
                    }
                }
            }
            else
            {
                result = base.ExecSpTable(name, parameters, timeOut);
            }

            return result;
        }

        public new DataTable ExecSpTable(string name, Hashtable parameters)
        {
            return base.ExecSpTable(name, parameters, 100, false);
        }

        /// <summary>
        /// Get top public message
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="siteid"></param>
        /// <param name="latestPubMsgID"></param>
        /// <param name="isTest"></param>
        /// <returns></returns>
        public DataTable Acc_Msg_GetTopPubMessage(int messageType, string siteId, int latestPubMsgID, bool isTest)
        {
            char[] separator = new char[] { ',' };
            string[] arraySite = siteId.Split(separator);

            DataTable result = null;
            DataTable returnTable = CreateDataTable();
            Hashtable htb = new Hashtable();
            htb.Add("messagetype", messageType);
            htb.Add("site1", arraySite[0]);
            htb.Add("site2", arraySite.Length > 1 ? arraySite[1] : string.Empty);
            htb.Add("site3", arraySite.Length > 2 ? arraySite[2] : string.Empty);
            htb.Add("latestmsgid", latestPubMsgID);
            htb.Add("istest", isTest);

            result = ExecSpTable("Acc_Msg_GetTopPubMessage", htb);

            if (result != null)
            {
                CopyDataTable(result, returnTable);
            }

            return returnTable;
        }

        /// <summary>
        /// Get top private message by user
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="latestPriMsgId"></param>
        /// <param name="isTest"></param>
        /// <returns></returns>
        public DataTable Dot_Msg_GetTopPriMessage24(int targetId, int latestPriMsgId, bool isTest)
        {
            DataTable result = null;
            DataTable returnTable = CreateDataTable();

            if (!this.Dot_Msg_HasPriMessage24(targetId, isTest))
            {
                return returnTable;
            }

            Hashtable htb2 = new Hashtable();
            htb2.Add("userid", targetId);
            htb2.Add("latestmsgid", latestPriMsgId);
            htb2.Add("istest", isTest);

            result = ExecSpTable("Dot_Msg_GetTopPriMessage24", htb2);

            if (result != null)
            {
                CopyDataTable(result, returnTable);
            }

            return returnTable;
        }

        /// <summary>
        /// Check if a user has some new private messages
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isTest"></param>
        /// <returns></returns>
        public bool Dot_Msg_HasPriMessage24(int userId, bool isTest)
        {
            int msgId = 0;
            DataTable result = null;
            Hashtable htb = new Hashtable();
            htb.Add("userid", userId);
            htb.Add("istest", isTest);
            result = ExecSpTable("Dot_Msg_HasPriMessage24", htb);
            if (result != null && result.Rows.Count > 0)
            {
                msgId = (int)result.Rows[0]["MessageID"];
            }

            return msgId > 0 ? true : false;
        }

        /// <summary>
        /// Get count of unread private messages by user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int Dot_Msg_GetPriMsgCount(int userID, string userName)
        {
            int count = 0;
            DataTable result = null;
            Hashtable htb = new Hashtable();
            htb.Add("userid", userID);
            htb.Add("username", userName);
            result = ExecSpTable("Dot_Msg_GetPriMsgCount", htb);

            // Check exist a DataTable in DataSet
            if (result != null && result.Rows.Count > 0)
            {
                count = (int)result.Rows[0][0];
            }

            return count;
        }

        /// <summary>
        /// Set unread private message is readed by user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        public bool Dot_Msg_SetPriMsgRead(int userID, string userName)
        {
            Hashtable htb = new Hashtable();
            htb.Add("userid", userID);
            htb.Add("username", userName);
            object result = ExecSp("Dot_Msg_SetPriMsgRead", htb);
            return result != null;
        }

        #endregion High-light and balloon messages

        #region Position taking message

        /// <summary>
        /// Get message of a customer.
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public DataTable Age_MessageNoticeSelByCustID(int custId)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);

            return base.ExecSpTable("Age_MessageNoticeSelByCustID", parameters);
        }

        /// <summary>
        /// Update message view status of a customer.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="isViewed"></param>
        public void Age_MessageNoticeUpdStatus(int custId, bool isViewed)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("isviewed", isViewed);

            ExecSp("Age_MessageNoticeUpdStatus", parameters);
        }

        /// <summary>
        /// Insert message into table Msg.
        /// </summary>
        public DataTable Dot_Msg_SetMessage(string content_en, string content_ch, string content_cs, string content_jp, string content_it, string content_th, string content_ko, string content_es, int msgType, bool isSticky, int creator, string creatorName, bool isTest, int matchId, bool isBatch)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("content_en", content_en);
            parameters.Add("content_ch", content_ch);
            parameters.Add("content_cs", content_cs);
            parameters.Add("content_jp", content_jp);
            parameters.Add("content_it", content_it);
            parameters.Add("content_th", content_th);
            parameters.Add("content_ko", content_ko);
            parameters.Add("content_es", content_es);
            parameters.Add("msgtype", msgType);
            parameters.Add("issticky", isSticky);
            parameters.Add("creator", creator);
            parameters.Add("creatorname", creatorName);
            parameters.Add("istest", isTest);
            parameters.Add("matchid", matchId);
            parameters.Add("isbatch", isBatch);
            parameters.Add("msgcontentstype", (byte)1);

            return base.ExecSpTable("Dot_Msg_SetMessage", parameters);
        }

        /// <summary>
        /// Insert message into table MsgTarget.
        /// </summary>
        public void Dot_Msg_SetUsrMessage(decimal messageId, int targetId, string targetName, bool isRead, string betTransId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("messageid", messageId);
            parameters.Add("targetid", targetId);
            parameters.Add("targetname", targetName);
            parameters.Add("isread", isRead);
            parameters.Add("bettransid", betTransId);

            ExecSp("Dot_Msg_SetUsrMessage", parameters);
        }

        public DataTable Age_MessageSelOne(byte roleId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("roleid", roleId);

            return base.ExecSpTable("Age_MessageSelOne", parameters);
        }

        #endregion Position taking message

        #region Announcement

        public DataTable Age_CustomerSelMessages(int targetID, string targetID2, bool isTest)
        {
            var htb = new Hashtable();
            htb.Add("targetid", targetID);
            htb.Add("targetid2", targetID2);
            htb.Add("istest", isTest);

            return this.ExecSpTable("Age_CustomerSelMessages", htb, 600);
        }

        public DataTable Age_CustomerSelMessages(int targetID, string targetID2, DateTime lastPostTime, int totalMessage, bool isTest)
        {
            var htb = new Hashtable();
            htb.Add("targetid", targetID);
            htb.Add("targetid2", targetID2);
            htb.Add("lastposttime", lastPostTime);
            htb.Add("totalmessage", totalMessage);
            htb.Add("istest", isTest);

            return this.ExecSpTable("Age_CustomerSelMessages", htb, 600);
        }

        public void Age_CustomerUpdMessagesRead(int userID, string msgIds)
        {
            var htb = new Hashtable();
            htb.Add("userid", userID);
            htb.Add("messageids", msgIds);

            ExecSp("Age_CustomerUpdMessagesRead", htb, 600);
        }

        #endregion Announcement

        #region Notification During Move-bet Process

        public DataTable GetNotificationDuringMoveBetsProcess()
        {
            return base.ExecSpTable("Age_MoveBetsProcessNotification", new Hashtable());
        }

        #endregion Notification During Move-bet Process

        private static object GetValue(DataRow row, string fieldname)
        {
            object returnObj = null;
            if (row.Table.Columns.Contains(fieldname) && (!row.IsNull(fieldname)))
            {
                returnObj = row[fieldname];
            }

            return returnObj;
        }

        private static void AddDataRow(DataRow row, DataTable msg)
        {
            string fieldName = string.Empty;
            DataRow dr = msg.NewRow();
            bool isBatch = false;
            string betTransId = string.Empty;

            DbValue<bool>.GetValue(row, "IsBatch", ref isBatch);
            DbValue<string>.GetValue(row, "BetTransID", ref betTransId);

            fieldName = "Content_ch";
            dr[fieldName] =
                isBatch ? DbValue<string>.GetValue(row, fieldName).Replace("@@", betTransId) : DbValue<string>.GetValue(row, fieldName);

            fieldName = "Content_cs";
            dr[fieldName] =
                isBatch ? DbValue<string>.GetValue(row, fieldName).Replace("@@", betTransId) : DbValue<string>.GetValue(row, fieldName);

            fieldName = "Content_en";
            dr[fieldName] =
                isBatch ? DbValue<string>.GetValue(row, fieldName).Replace("@@", betTransId) : DbValue<string>.GetValue(row, fieldName);

            fieldName = "Content_it";
            dr[fieldName] =
                isBatch ? DbValue<string>.GetValue(row, fieldName).Replace("@@", betTransId) : DbValue<string>.GetValue(row, fieldName);

            fieldName = "Content_jp";
            dr[fieldName] =
                isBatch ? DbValue<string>.GetValue(row, fieldName).Replace("@@", betTransId) : DbValue<string>.GetValue(row, fieldName);

            fieldName = "Content_ko";
            dr[fieldName] =
                isBatch ? DbValue<string>.GetValue(row, fieldName).Replace("@@", betTransId) : DbValue<string>.GetValue(row, fieldName);

            fieldName = "Content_th";
            dr[fieldName] =
                isBatch ? DbValue<string>.GetValue(row, fieldName).Replace("@@", betTransId) : DbValue<string>.GetValue(row, fieldName);

            fieldName = "Content_vn";
            string contentVN = string.Empty + DbValue<string>.GetValue(row, fieldName);
            dr[fieldName] =
                isBatch ? contentVN.Replace("@@", betTransId) : contentVN;

            fieldName = "CreateTime";
            dr[fieldName] = DbValue<DateTime>.GetValue(row, fieldName);

            fieldName = "Creator";
            dr["CreatorId"] = DbValue<int>.GetValue(row, fieldName);

            fieldName = "CreatorName";
            dr[fieldName] = DbValue<string>.GetValue(row, fieldName);

            fieldName = "IsSticky";
            dr[fieldName] = DbValue<bool>.GetValue(row, fieldName);

            fieldName = "LastUpdateName";
            dr[fieldName] = DbValue<string>.GetValue(row, fieldName);

            fieldName = "LastUpdateTime";
            dr[fieldName] = DbValue<DateTime>.GetValue(row, fieldName);

            fieldName = "LastUpdator";
            dr[fieldName] = DbValue<int>.GetValue(row, fieldName);

            fieldName = "MessageId";
            dr[fieldName] = DbValue<int>.GetValue(row, fieldName);

            fieldName = "MsgType";
            dr["MessageType"] = DbValue<byte>.GetValue(row, fieldName);

            fieldName = "PostTime";
            dr[fieldName] = DbValue<DateTime>.GetValue(row, fieldName);

            fieldName = "TargetId";
            dr["UserId"] = DbValue<int>.GetValue(row, fieldName);

            fieldName = "UserName";
            dr[fieldName] = DbValue<string>.GetValue(row, fieldName);

            msg.Rows.Add(dr);
        }
    }
}