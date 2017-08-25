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