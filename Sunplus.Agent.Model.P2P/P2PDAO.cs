namespace Sunplus.Agent.Model.DL.MemberInfo.P2P
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data.DNAide;
    using Sunplus.Agent.Model.DL.Base;

    [Obsolete("Should rename to Sunplus.Agent.Model.P2P")]
    public class P2PDAO : DAOBase
    {
        #region Gets infos
        public DataSet Acc_CustomerSelP2PSuper_All(int custId, out int errCode, out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerSelP2PSuper_All", htb);
            var ds = ret.ResultSet;

            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);

            return ds;
        }

        public DataSet Acc_CustomerSelP2PMaster_All(int custId, out int errCode, out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerSelP2PMaster_All", htb);
            var ds = ret.ResultSet;

            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);

            return ds;
        }

        public DataSet Acc_CustomerSelP2PAgent_All(int custId, out int errCode, out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerSelP2PAgent_All", htb);
            var ds = ret.ResultSet;

            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);

            return ds;
        }

        public DataSet Acc_CustomerSelP2PMember_All(int custId, out int errCode, out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerSelP2PMember_All", htb);
            var ds = ret.ResultSet;

            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);

            return ds;
        }

        public DataTable Age_CustomerSelP2PComm_SAg(int custId)
        {
            var htb = new Hashtable();
            htb.Add("srecommend", custId);

            return ExecSpTable("Age_CustomerSelP2PComm_SAg", htb);
        }

        public DataTable Age_CustomerSelP2PComm_MAg(int custId)
        {
            var htb = new Hashtable();
            htb.Add("mrecommend", custId);

            return ExecSpTable("Age_CustomerSelP2PComm_MAg", htb);
        }

        public DataTable Age_CustomerSelP2PComm_Ag(int custId)
        {
            var htb = new Hashtable();
            htb.Add("recommend", custId);

            return ExecSpTable("Age_CustomerSelP2PComm_Ag", htb);
        }
        #endregion

        #region Updates super comm/game setting/max transfer
        public void Acc_CustomerUpdP2PSuper_Commission(
                int custId,  
                decimal v253discount_1,
                decimal v253discount_2,                  
                int subAccId,
                int userId,   
                string language,  
                string ip,  
                out int errCode,  
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("v253discount_1", v253discount_1);
            htb.Add("v253discount_2", v253discount_2);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PSuper_Commission", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }

        public void Acc_CustomerUpdP2PSuper_GameSetting(
                int custId,  
                bool stakeLevel1_1001,   
                bool stakeLevel2_1001,   
                bool stakeLevel3_1001,   
                bool stakeLevel4_1001,   
                bool stakeLevel5_1001,                     
                bool stakeLevel1_1002,   
                bool stakeLevel2_1002,   
                bool stakeLevel3_1002,   
                bool stakeLevel4_1002,   
                bool stakeLevel5_1002,                     
                int subAccId,
                int userId,   
                string language,  
                string ip,  
                out int errCode,  
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("stakelevel1_1001", stakeLevel1_1001);
            htb.Add("stakelevel2_1001", stakeLevel2_1001);
            htb.Add("stakelevel3_1001", stakeLevel3_1001);
            htb.Add("stakelevel4_1001", stakeLevel4_1001);
            htb.Add("stakelevel5_1001", stakeLevel5_1001);
            htb.Add("stakelevel1_1002", stakeLevel1_1002);
            htb.Add("stakelevel2_1002", stakeLevel2_1002);
            htb.Add("stakelevel3_1002", stakeLevel3_1002);
            htb.Add("stakelevel4_1002", stakeLevel4_1002);
            htb.Add("stakelevel5_1002", stakeLevel5_1002);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PSuper_GameSetting", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }

        public void Acc_CustomerUpdP2PSuper_MaxTransfer(
                int custId,  
                decimal maxTransfer,  
                bool isDailyAutoReset,                    
                int subAccId,
                int userId,   
                string language,  
                string ip,  
                out int errCode,  
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("maxtransfer", maxTransfer);
            htb.Add("isdailyautoreset", isDailyAutoReset);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PSuper_MaxTransfer", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }
        #endregion

        #region Updates master comm/game setting/max transfer
        public void Acc_CustomerUpdMaster_P2PCommission(
                int custId,  
                decimal v253discount_1,
                decimal v253discount_2,                  
                int subAccId,
                int userId,   
                string language,  
                string ip,  
                out int errCode,  
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("v253discount_1", v253discount_1);
            htb.Add("v253discount_2", v253discount_2);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PMaster_Commission", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }

        public void Acc_CustomerUpdMaster_P2PGameSetting(
                int custId,  
                bool stakeLevel1_1001,   
                bool stakeLevel2_1001,   
                bool stakeLevel3_1001,   
                bool stakeLevel4_1001,   
                bool stakeLevel5_1001,                     
                bool stakeLevel1_1002,   
                bool stakeLevel2_1002,   
                bool stakeLevel3_1002,   
                bool stakeLevel4_1002,   
                bool stakeLevel5_1002,                     
                int subAccId,
                int userId,   
                string language,  
                string ip,  
                out int errCode,  
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("stakelevel1_1001", stakeLevel1_1001);
            htb.Add("stakelevel2_1001", stakeLevel2_1001);
            htb.Add("stakelevel3_1001", stakeLevel3_1001);
            htb.Add("stakelevel4_1001", stakeLevel4_1001);
            htb.Add("stakelevel5_1001", stakeLevel5_1001);
            htb.Add("stakelevel1_1002", stakeLevel1_1002);
            htb.Add("stakelevel2_1002", stakeLevel2_1002);
            htb.Add("stakelevel3_1002", stakeLevel3_1002);
            htb.Add("stakelevel4_1002", stakeLevel4_1002);
            htb.Add("stakelevel5_1002", stakeLevel5_1002);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PMaster_GameSetting", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }

        public void Acc_CustomerUpdMaster_P2PMaxTransfer(
                int custId,  
                decimal maxTransfer,  
                bool isDailyAutoReset,                    
                int subAccId,
                int userId,   
                string language,  
                string ip,  
                out int errCode,  
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("maxtransfer", maxTransfer);
            htb.Add("isdailyautoreset", isDailyAutoReset);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PMaster_MaxTransfer", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }
        #endregion

        #region Updates agent comm/game setting/max transfer
        public void Acc_CustomerUpdAgent_P2PCommission(
                int custId,
                decimal v253discount_1,
                decimal v253discount_2,
                int subAccId,
                int userId,
                string language,
                string ip,
                out int errCode,
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("v253discount_1", v253discount_1);
            htb.Add("v253discount_2", v253discount_2);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PAgent_Commission", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }

        public void Acc_CustomerUpdAgent_P2PGameSetting(
                int custId,
                bool stakeLevel1_1001,
                bool stakeLevel2_1001,
                bool stakeLevel3_1001,
                bool stakeLevel4_1001,
                bool stakeLevel5_1001,
                bool stakeLevel1_1002,
                bool stakeLevel2_1002,
                bool stakeLevel3_1002,
                bool stakeLevel4_1002,
                bool stakeLevel5_1002,
                int subAccId,
                int userId,
                string language,
                string ip,
                out int errCode,
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("stakelevel1_1001", stakeLevel1_1001);
            htb.Add("stakelevel2_1001", stakeLevel2_1001);
            htb.Add("stakelevel3_1001", stakeLevel3_1001);
            htb.Add("stakelevel4_1001", stakeLevel4_1001);
            htb.Add("stakelevel5_1001", stakeLevel5_1001);
            htb.Add("stakelevel1_1002", stakeLevel1_1002);
            htb.Add("stakelevel2_1002", stakeLevel2_1002);
            htb.Add("stakelevel3_1002", stakeLevel3_1002);
            htb.Add("stakelevel4_1002", stakeLevel4_1002);
            htb.Add("stakelevel5_1002", stakeLevel5_1002);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PAgent_GameSetting", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }

        public void Acc_CustomerUpdAgent_P2PMaxTransfer(
                int custId,
                decimal maxTransfer,
                bool isDailyAutoReset,
                int subAccId,
                int userId,
                string language,
                string ip,
                out int errCode,
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("maxtransfer", maxTransfer);
            htb.Add("isdailyautoreset", isDailyAutoReset);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PAgent_MaxTransfer", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }
        #endregion

        #region Updates member comm/game setting/max transfer
        public void Acc_CustomerUpdMember_P2PCommission(
                int custId,
                decimal v253discount_1,
                decimal v253discount_2,
                int subAccId,
                int userId,
                string language,
                string ip,
                out int errCode,
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("v253discount_1", v253discount_1);
            htb.Add("v253discount_2", v253discount_2);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PMember_Commission", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }

        public void Acc_CustomerUpdMember_P2PGameSetting(
                int custId,
                bool stakeLevel1_1001,
                bool stakeLevel2_1001,
                bool stakeLevel3_1001,
                bool stakeLevel4_1001,
                bool stakeLevel5_1001,
                bool stakeLevel1_1002,
                bool stakeLevel2_1002,
                bool stakeLevel3_1002,
                bool stakeLevel4_1002,
                bool stakeLevel5_1002,
                int subAccId,
                int userId,
                string language,
                string ip,
                out int errCode,
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("stakelevel1_1001", stakeLevel1_1001);
            htb.Add("stakelevel2_1001", stakeLevel2_1001);
            htb.Add("stakelevel3_1001", stakeLevel3_1001);
            htb.Add("stakelevel4_1001", stakeLevel4_1001);
            htb.Add("stakelevel5_1001", stakeLevel5_1001);
            htb.Add("stakelevel1_1002", stakeLevel1_1002);
            htb.Add("stakelevel2_1002", stakeLevel2_1002);
            htb.Add("stakelevel3_1002", stakeLevel3_1002);
            htb.Add("stakelevel4_1002", stakeLevel4_1002);
            htb.Add("stakelevel5_1002", stakeLevel5_1002);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PMember_GameSetting", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }

        public void Acc_CustomerUpdMember_P2PMaxTransfer(
                int custId,
                decimal maxTransfer,
                bool isDailyAutoReset,
                int subAccId,
                int userId,
                string language,
                string ip,
                out int errCode,
                out string errMsg)
        {
            errCode = 0;
            errMsg = string.Empty;

            var htb = new Hashtable();
            htb.Add("custid", custId);
            htb.Add("maxtransfer", maxTransfer);
            htb.Add("isdailyautoreset", isDailyAutoReset);
            htb.Add("subaccid", subAccId);
            htb.Add("userid", userId);
            htb.Add("language", language);
            htb.Add("ip", ip);
            htb.Add("errcode", errCode);
            htb.Add("errmsg", errMsg);

            var ret = ExecSp("Acc_CustomerUpdP2PMember_MaxTransfer", htb);
            errCode = Convert.ToInt32(ret.Outputs["errcode"]);
            errMsg = Convert.ToString(ret.Outputs["errmsg"]);
        }
        #endregion

        #region New P2P updates for game setting

        #region Super
        #region Update single super
        public void Acc_CustomerUpdSuper_P2PGameSetting(
                int custId,
                int subAccId,
                string ip,
                int userId,
                string language,
                IList<int> bettypeIds,
                string vals,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            var pars = vals;
            var items = pars.Split(';');

            var list = new List<Hashtable>();
            var cmd = new Hashtable();
            var counter = 0;

            foreach (string item in items)
            {
                var values = item.Split(',');
                var index = 0;

                cmd = new Hashtable();
                cmd.Add("custid", custId);
                cmd.Add("subaccid", subAccId);
                cmd.Add("ip", ip);
                cmd.Add("userid", userId);
                cmd.Add("language", language);
                cmd.Add("errcode", 0);
                cmd.Add("errmsg", string.Empty);
                cmd.Add("bettype", bettypeIds.ElementAt(counter));

                foreach (string value in values)
                {
                    index++;

                    var val = value.Replace("[", string.Empty)
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty)
                        .Replace("]", string.Empty);

                    cmd.Add("stakelevel" + index, val);
                }

                list.Add(cmd);

                counter++;
            }

            var ret = ExecSp("Acc_CustomerUpdP2PSuper_GameSetting", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion
        #region Update multi-supers
        public void Acc_CustomerUpdSuper_P2PGameSetting(
                IList<int> custIds,
                IList<int> subAccIds,
                string ip,
                IList<int> userIds,
                string language,
                IList<int> bettypeIds,
                string pars,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();
                        
            var items = pars.Split(';');

            IList<Hashtable> list = new List<Hashtable>();            
            IList<string> sps = new List<string>();

            var cmd = new Hashtable();
            var counter = 0;

            for (var i = 0; i < custIds.Count; i++)
            {
                counter = 0;

                foreach (string item in items)
                {
                    var values = item.Split(',');
                    var index = 0;

                    cmd = new Hashtable();
                    cmd.Add("custid", custIds.ElementAt(i));
                    cmd.Add("subaccid", subAccIds.ElementAt(i));
                    cmd.Add("ip", ip);
                    cmd.Add("userid", userIds.ElementAt(i));
                    cmd.Add("language", language);
                    cmd.Add("errcode", 0);
                    cmd.Add("errmsg", string.Empty);
                    cmd.Add("bettype", bettypeIds.ElementAt(counter));

                    foreach (string value in values)
                    {
                        index++;

                        var val = value.Replace("[", string.Empty)
                            .Replace("{", string.Empty)
                            .Replace("}", string.Empty)
                            .Replace("]", string.Empty);

                        cmd.Add("stakelevel" + index, val);
                    }

                    list.Add(cmd);

                    sps.Add("Acc_CustomerUpdP2PSuper_GameSetting");

                    counter++;
                }
            }

            var ret = ExecSp(sps, list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion
        #endregion

        #region Master
        #region Update single master
        public void Acc_CustomerUpdMaster_P2PGameSetting(
                int custId,
                int subAccId,
                string ip,
                int userId,
                string language,                
                IList<int> bettypeIds,
                string vals,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            var pars = vals;
            var items = pars.Split(';');

            var list = new List<Hashtable>();
            var cmd = new Hashtable();
            var counter = 0;

            foreach (string item in items)
            {
                var values = item.Split(',');
                var index = 0;

                cmd = new Hashtable();
                cmd.Add("custid", custId);
                cmd.Add("subaccid", subAccId);
                cmd.Add("ip", ip);
                cmd.Add("userid", userId);
                cmd.Add("language", language);
                cmd.Add("errcode", 0);
                cmd.Add("errmsg", string.Empty);
                cmd.Add("bettype", bettypeIds.ElementAt(counter));

                foreach (string value in values)
                {
                    index++;

                    var val = value.Replace("[", string.Empty)
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty)
                        .Replace("]", string.Empty);

                    cmd.Add("stakelevel" + index, val);
                }

                list.Add(cmd);

                counter++;
            }

            var ret = ExecSp("Acc_CustomerUpdP2PMaster_GameSetting", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion
        #region Update multi-masters
        public void Acc_CustomerUpdMaster_P2PGameSetting(
                IList<int> custIds,
                IList<int> subAccIds,
                string ip,
                IList<int> userIds,
                string language,
                IList<int> bettypeIds,
                string pars,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            var items = pars.Split(';');

            IList<Hashtable> list = new List<Hashtable>();
            IList<string> sps = new List<string>();

            var cmd = new Hashtable();
            var counter = 0;

            for (var i = 0; i < custIds.Count; i++)
            {
                counter = 0;

                foreach (string item in items)
                {
                    var values = item.Split(',');
                    var index = 0;

                    cmd = new Hashtable();
                    cmd.Add("custid", custIds.ElementAt(i));
                    cmd.Add("subaccid", subAccIds.ElementAt(i));
                    cmd.Add("ip", ip);
                    cmd.Add("userid", userIds.ElementAt(i));
                    cmd.Add("language", language);
                    cmd.Add("errcode", 0);
                    cmd.Add("errmsg", string.Empty);
                    cmd.Add("bettype", bettypeIds.ElementAt(counter));

                    foreach (string value in values)
                    {
                        index++;

                        var val = value.Replace("[", string.Empty)
                            .Replace("{", string.Empty)
                            .Replace("}", string.Empty)
                            .Replace("]", string.Empty);

                        cmd.Add("stakelevel" + index, val);
                    }

                    list.Add(cmd);

                    sps.Add("Acc_CustomerUpdP2PMaster_GameSetting");

                    counter++;
                }
            }

            var ret = ExecSp(sps, list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion
        #endregion

        #region Agent
        #region Update single agent
        public void Acc_CustomerUpdAgent_P2PGameSetting(
                int custId,
                int subAccId,
                string ip,
                int userId,
                string language,
                IList<int> bettypeIds,
                string vals,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            var pars = vals;
            var items = pars.Split(';');

            var list = new List<Hashtable>();
            var cmd = new Hashtable();
            var counter = 0;

            foreach (string item in items)
            {
                var values = item.Split(',');
                var index = 0;

                cmd = new Hashtable();
                cmd.Add("custid", custId);
                cmd.Add("subaccid", subAccId);
                cmd.Add("ip", ip);
                cmd.Add("userid", userId);
                cmd.Add("language", language);
                cmd.Add("errcode", 0);
                cmd.Add("errmsg", string.Empty);
                cmd.Add("bettype", bettypeIds.ElementAt(counter));

                foreach (string value in values)
                {
                    index++;

                    var val = value.Replace("[", string.Empty)
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty)
                        .Replace("]", string.Empty);

                    cmd.Add("stakelevel" + index, val);
                }

                list.Add(cmd);

                counter++;
            }

            var ret = ExecSp("Acc_CustomerUpdP2PAgent_GameSetting", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion
        #region Update multi-agents
        public void Acc_CustomerUpdAgent_P2PGameSetting(
                IList<int> custIds,
                IList<int> subAccIds,
                string ip,
                IList<int> userIds,
                string language,
                IList<int> bettypeIds,
                string pars,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            var items = pars.Split(';');

            IList<Hashtable> list = new List<Hashtable>();
            IList<string> sps = new List<string>();

            var cmd = new Hashtable();
            var counter = 0;

            for (var i = 0; i < custIds.Count; i++)
            {
                counter = 0;

                foreach (string item in items)
                {
                    var values = item.Split(',');
                    var index = 0;

                    cmd = new Hashtable();
                    cmd.Add("custid", custIds.ElementAt(i));
                    cmd.Add("subaccid", subAccIds.ElementAt(i));
                    cmd.Add("ip", ip);
                    cmd.Add("userid", userIds.ElementAt(i));
                    cmd.Add("language", language);
                    cmd.Add("errcode", 0);
                    cmd.Add("errmsg", string.Empty);
                    cmd.Add("bettype", bettypeIds.ElementAt(counter));

                    foreach (string value in values)
                    {
                        index++;

                        var val = value.Replace("[", string.Empty)
                            .Replace("{", string.Empty)
                            .Replace("}", string.Empty)
                            .Replace("]", string.Empty);

                        cmd.Add("stakelevel" + index, val);
                    }

                    list.Add(cmd);

                    sps.Add("Acc_CustomerUpdP2PAgent_GameSetting");

                    counter++;
                }
            }

            var ret = ExecSp(sps, list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion
        #endregion

        #region Member
        #region Update single member
        public void Acc_CustomerUpdMember_P2PGameSetting(
                int custId,
                int subAccId,
                string ip,
                int userId,
                string language,
                IList<int> bettypeIds,
                string vals,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            var pars = vals;
            var items = pars.Split(';');

            var list = new List<Hashtable>();
            var cmd = new Hashtable();
            var counter = 0;

            foreach (string item in items)
            {
                var values = item.Split(',');
                var index = 0;

                cmd = new Hashtable();
                cmd.Add("custid", custId);
                cmd.Add("subaccid", subAccId);
                cmd.Add("ip", ip);
                cmd.Add("userid", userId);
                cmd.Add("language", language);
                cmd.Add("errcode", 0);
                cmd.Add("errmsg", string.Empty);
                cmd.Add("bettype", bettypeIds.ElementAt(counter));

                foreach (string value in values)
                {
                    index++;

                    var val = value.Replace("[", string.Empty)
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty)
                        .Replace("]", string.Empty);

                    cmd.Add("stakelevel" + index, val);
                }

                list.Add(cmd);

                counter++;
            }

            var ret = ExecSp("Acc_CustomerUpdP2PMember_GameSetting", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion
        #region Update multi-members
        public void Acc_CustomerUpdMember_P2PGameSetting(
                IList<int> custIds,
                IList<int> subAccIds,
                string ip,
                IList<int> userIds,
                string language,
                IList<int> bettypeIds,
                string pars,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            var items = pars.Split(';');

            IList<Hashtable> list = new List<Hashtable>();
            IList<string> sps = new List<string>();

            var cmd = new Hashtable();
            var counter = 0;

            for (var i = 0; i < custIds.Count; i++)
            {
                counter = 0;

                foreach (string item in items)
                {
                    var values = item.Split(',');
                    var index = 0;

                    cmd = new Hashtable();
                    cmd.Add("custid", custIds.ElementAt(i));
                    cmd.Add("subaccid", subAccIds.ElementAt(i));
                    cmd.Add("ip", ip);
                    cmd.Add("userid", userIds.ElementAt(i));
                    cmd.Add("language", language);
                    cmd.Add("errcode", 0);
                    cmd.Add("errmsg", string.Empty);
                    cmd.Add("bettype", bettypeIds.ElementAt(counter));

                    foreach (string value in values)
                    {
                        index++;

                        var val = value.Replace("[", string.Empty)
                            .Replace("{", string.Empty)
                            .Replace("}", string.Empty)
                            .Replace("]", string.Empty);

                        cmd.Add("stakelevel" + index, val);
                    }

                    list.Add(cmd);

                    sps.Add("Acc_CustomerUpdP2PMember_GameSetting");

                    counter++;
                }
            }

            var ret = ExecSp(sps, list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion
        #endregion

        #endregion

        #region New P2P updates for max transfer

        #region Super
        public void Acc_CustomerUpdP2PSuper_MaxTransfer(
                IList<int> custIds,
                decimal maxTransfer,
                bool isDailyAutoReset,
                IList<int> subAccIds,
                IList<int> userIds,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            IList<Hashtable> list = new List<Hashtable>();

            for (var i = 0; i < custIds.Count; i++)
            {
                var htb = new Hashtable();
                htb.Add("custid", custIds.ElementAt(i));
                htb.Add("maxtransfer", maxTransfer);
                htb.Add("isdailyautoreset", isDailyAutoReset);
                htb.Add("subaccid", subAccIds.ElementAt(i));
                htb.Add("userid", userIds.ElementAt(i));
                htb.Add("language", language);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                list.Add(htb);
            }

            var ret = ExecSp("Acc_CustomerUpdP2PSuper_MaxTransfer", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion

        #region Master
        public void Acc_CustomerUpdP2PMaster_MaxTransfer(
                IList<int> custIds,
                decimal maxTransfer,
                bool isDailyAutoReset,
                IList<int> subAccIds,
                IList<int> userIds,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            IList<Hashtable> list = new List<Hashtable>();

            for (var i = 0; i < custIds.Count; i++)
            {
                var htb = new Hashtable();
                htb.Add("custid", custIds.ElementAt(i));
                htb.Add("maxtransfer", maxTransfer);
                htb.Add("isdailyautoreset", isDailyAutoReset);
                htb.Add("subaccid", subAccIds.ElementAt(i));
                htb.Add("userid", userIds.ElementAt(i));
                htb.Add("language", language);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                list.Add(htb);
            }

            var ret = ExecSp("Acc_CustomerUpdP2PMaster_MaxTransfer", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion

        #region Agent
        public void Acc_CustomerUpdP2PAgent_MaxTransfer(
                IList<int> custIds,
                decimal maxTransfer,
                bool isDailyAutoReset,
                IList<int> subAccIds,
                IList<int> userIds,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            IList<Hashtable> list = new List<Hashtable>();

            for (var i = 0; i < custIds.Count; i++)
            {
                var htb = new Hashtable();
                htb.Add("custid", custIds.ElementAt(i));
                htb.Add("maxtransfer", maxTransfer);
                htb.Add("isdailyautoreset", isDailyAutoReset);
                htb.Add("subaccid", subAccIds.ElementAt(i));
                htb.Add("userid", userIds.ElementAt(i));
                htb.Add("language", language);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                list.Add(htb);
            }

            var ret = ExecSp("Acc_CustomerUpdP2PAgent_MaxTransfer", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion

        #region Member
        public void Acc_CustomerUpdP2PMember_MaxTransfer(
                IList<int> custIds,
                decimal maxTransfer,
                bool isDailyAutoReset,
                IList<int> subAccIds,
                IList<int> userIds,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            IList<Hashtable> list = new List<Hashtable>();

            for (var i = 0; i < custIds.Count; i++)
            {
                var htb = new Hashtable();
                htb.Add("custid", custIds.ElementAt(i));
                htb.Add("maxtransfer", maxTransfer);
                htb.Add("isdailyautoreset", isDailyAutoReset);
                htb.Add("subaccid", subAccIds.ElementAt(i));
                htb.Add("userid", userIds.ElementAt(i));
                htb.Add("language", language);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                list.Add(htb);
            }

            var ret = ExecSp("Acc_CustomerUpdP2PMember_MaxTransfer", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion

        #endregion

        #region New P2P updates for commission

        #region Super
        public void Acc_CustomerUpdP2PSuper_Commission(
                IList<int> custIds,
                decimal v253discount_1,
                decimal v253discount_2,
                IList<int> subAccIds,
                IList<int> userIds,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            IList<Hashtable> list = new List<Hashtable>();

            for (var i = 0; i < custIds.Count; i++)
            {
                var htb = new Hashtable();
                htb.Add("custid", custIds.ElementAt(i));
                htb.Add("v253discount_1", v253discount_1);
                htb.Add("v253discount_2", v253discount_2);
                htb.Add("subaccid", subAccIds.ElementAt(i));
                htb.Add("userid", userIds.ElementAt(i));
                htb.Add("language", language);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                list.Add(htb);
            }

            var ret = ExecSp("Acc_CustomerUpdP2PSuper_Commission", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion

        #region Master
        public void Acc_CustomerUpdP2PMaster_Commission(
                IList<int> custIds,
                decimal v253discount_1,
                decimal v253discount_2,
                IList<int> subAccIds,
                IList<int> userIds,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            IList<Hashtable> list = new List<Hashtable>();

            for (var i = 0; i < custIds.Count; i++)
            {
                var htb = new Hashtable();
                htb.Add("custid", custIds.ElementAt(i));
                htb.Add("v253discount_1", v253discount_1);
                htb.Add("v253discount_2", v253discount_2);
                htb.Add("subaccid", subAccIds.ElementAt(i));
                htb.Add("userid", userIds.ElementAt(i));
                htb.Add("language", language);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                list.Add(htb);
            }

            var ret = ExecSp("Acc_CustomerUpdP2PMaster_Commission", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion

        #region Agent
        public void Acc_CustomerUpdP2PAgent_Commission(
                IList<int> custIds,
                decimal v253discount_1,
                decimal v253discount_2,
                IList<int> subAccIds,
                IList<int> userIds,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            IList<Hashtable> list = new List<Hashtable>();

            for (var i = 0; i < custIds.Count; i++)
            {
                var htb = new Hashtable();
                htb.Add("custid", custIds.ElementAt(i));
                htb.Add("v253discount_1", v253discount_1);
                htb.Add("v253discount_2", v253discount_2);
                htb.Add("subaccid", subAccIds.ElementAt(i));
                htb.Add("userid", userIds.ElementAt(i));
                htb.Add("language", language);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                list.Add(htb);
            }

            var ret = ExecSp("Acc_CustomerUpdP2PAgent_Commission", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion

        #region Member
        public void Acc_CustomerUpdP2PMember_Commission(
                IList<int> custIds,
                decimal v253discount_1,
                decimal v253discount_2,
                IList<int> subAccIds,
                IList<int> userIds,
                string language,
                string ip,
                out IList<int> errCodes,
                out IList<string> errMsgs)
        {
            errCodes = new List<int>();
            errMsgs = new List<string>();

            IList<Hashtable> list = new List<Hashtable>();

            for (var i = 0; i < custIds.Count; i++)
            {
                var htb = new Hashtable();
                htb.Add("custid", custIds.ElementAt(i));
                htb.Add("v253discount_1", v253discount_1);
                htb.Add("v253discount_2", v253discount_2);
                htb.Add("subaccid", subAccIds.ElementAt(i));
                htb.Add("userid", userIds.ElementAt(i));
                htb.Add("language", language);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                list.Add(htb);
            }

            var ret = ExecSp("Acc_CustomerUpdP2PMember_Commission", list, false);

            foreach (DbResult dbr in ret)
            {
                errCodes.Add(Convert.ToInt32(dbr.Outputs["errcode"]));
                errMsgs.Add(Convert.ToString(dbr.Outputs["errmsg"]));
            }
        }
        #endregion

        #endregion
    }
}
