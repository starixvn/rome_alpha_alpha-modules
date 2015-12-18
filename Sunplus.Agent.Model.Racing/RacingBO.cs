namespace Sunplus.Agent.Model.BL.Racing
{
    using System;
    using System.Collections;
    using System.Data;
    using Sunplus.Agent.Model.DL.Racing;

    [Obsolete("Should rename to Sunplus.Agent.Model.Racing")]
    public class RacingBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        public RacingBO(string netSessionId)
            : base(netSessionId)
        { 
        }
               
        /// <summary>
        /// Get HR Setting for member
        /// </summary>
        public DataSet GetHRSetting_Member(int custId, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerSelHRMember_All(custId, out outputs);
        }

        /// <summary>
        /// Get HR Setting for Agent
        /// </summary>
        public DataSet GetHRSetting_Agent(int custId, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerSelHRAgent_All(custId, out outputs);  
        }

        /// <summary>
        /// Get HR Setting for Master
        /// </summary>
        public DataSet GetHRSetting_Master(int custId, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerSelHRMaster_All(custId, out outputs);
        }

        /// <summary>
        /// Get HR Setting for Super
        /// </summary>
        public DataSet GetHRSetting_Super(int custId, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerSelHRSuper_All(custId, out outputs);
        }

        /// <summary>
        /// Super Get HR Setting of Agent
        /// </summary>
        public DataSet SuperGetHRSetting_Agent(int custId, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerSelHRAgent_SAg(custId, out outputs);
        }

        /// <summary>
        /// Super update Master HR Setting
        /// </summary>
        public DataSet SuperUpdateMasterHR(Hashtable parameters, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerUpdMasterHR_All(parameters, out outputs);
        }

        /// <summary>
        /// Master update Agent HR Setting
        /// </summary>
        public DataSet MasterUpdateAgentHR(Hashtable parameters, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerUpdAgentHR_All(parameters, out outputs);
        }

        /// <summary>
        /// Agent update Member HR Setting
        /// </summary>
        public DataSet AgentUpdateMemberHR(Hashtable parameters, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerUpdMemberHR_All(parameters, out outputs);
        }

        /// <summary>
        /// Super update member HR Setting
        /// </summary>
        public DataSet SuperUpdateMemberHR(Hashtable parameters, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerUpdMemberHR_SAg(parameters, out outputs);
        }

        /// <summary>
        /// Master update member HR Setting
        /// </summary>
        public DataSet MasterUpdateMemberHR(Hashtable parameters, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerUpdMemberHR_MAg(parameters, out outputs);
        }

        /// <summary>
        /// Super get HR Setting for Member
        /// </summary>
        public DataSet Super_GetHRSetting_Member(int custId, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerSelHREditMember_SAg(custId, out outputs);
        }

        /// <summary>
        /// Master get HR Setting for Member
        /// </summary>
        public DataSet Master_GetHRSetting_Member(int custId, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerSelHREditMember_MAg(custId, out outputs);
        }

        /// <summary>
        /// Super get min HR PT
        /// </summary>
        public DataSet SuperGetMinPTHR(int custId, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerSelHRComEditOverride_MAg(custId, out outputs);
        }

        /// <summary>
        /// Master get min HR PT
        /// </summary>
        public DataSet MasterGetMinPTHR(int custId, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustomerSelHRComEditOverride_Ag(custId, out outputs);
        }

        /// <summary>
        /// Update Min HR PT
        /// </summary>
        public DataSet UpdateMinPTHR(Hashtable parameters, out Hashtable outputs)
        {
            RacingDAO dao = new RacingDAO();
            return dao.Acc_CustOverrideUpdHR(parameters, out outputs); 
        }

        /// <summary>
        /// Update racing status.
        /// </summary>
        /// <param name="parameters"> 
        /// @custid int,
        /// @updatedisable151_31 bit = 0,  
        /// @updatedisable151_41 bit = 0,  
        /// @updatedisable151_51 bit = 0,  
        /// @updatedisable152_41 bit = 0,  
        /// @updatedisable152_51 bit = 0,  
        /// @updatedisable153_51 bit = 0,  
   
        /// @disable151_31 bit = 0,  
        /// @disabledownlines151_31 bit = 0 ,  
        /// @disable151_41 bit = 0,  
        /// @disabledownlines151_41 bit = 0 ,  
        /// @disable151_51 bit = 0,  
        /// @disabledownlines151_51 bit = 0 ,  
        /// @disable152_41 bit = 0,  
        /// @disabledownlines152_41 bit = 0 ,  
        /// @disable152_51 bit = 0,  
        /// @disabledownlines152_51 bit = 0 ,  
        /// @disable153_51 bit = 0,  
        /// @disabledownlines153_51 bit = 0,
        /// @ip
        /// </param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void UpdateStatus(Hashtable parameters, out string errCode, out string errMsg)
        {
            RacingDAO dao = new RacingDAO();

            parameters.Add("subaccid", CurrentSession.SubAccId);
            parameters.Add("userid", CurrentSession.UserId);
            parameters.Add("language", CurrentSession.LangKey);

            dao.Acc_CustomerUpdRBStatus(parameters, out errCode, out errMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public DataTable SelectStatus(int custId)
        { 
            return (new RacingDAO()).Acc_CustomerSelRBStatus(custId);
        }
    }
}