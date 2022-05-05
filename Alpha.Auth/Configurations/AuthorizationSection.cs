namespace Alpha.Auth.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class AuthorizationSection : ConfigurationSection
    {
        private static AuthorizationSection instance
           = ConfigurationManager.GetSection("AuthorizationSection") as AuthorizationSection;

        private static IEnumerable<GroupPageAccessElement> groupPageAccesses = instance?.GroupPageAccesses.Cast<GroupPageAccessElement>();

        private static string[] custIdsQueryStringParamNames;
        private static string[] custIdQueryStringParamNames;
        private static string[] custIdJsonParamNames;
        private static string[] splitedIgnoreUrls;

        public static AuthorizationSection Instance
        {
            get
            {
                return instance;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public GroupPageAccessCollection GroupPageAccesses
        {
            get
            {
                return (GroupPageAccessCollection)base[string.Empty];
            }
        }

        public IEnumerable<GroupPageAccessElement> GroupPageAccessList
        {
            get
            {
                return groupPageAccesses;
            }

            internal set
            {
#pragma warning disable S2696 // Instance members should not write to "static" fields
                groupPageAccesses = value;
#pragma warning restore S2696 // Instance members should not write to "static" fields
            }
        }

        [ConfigurationProperty("fullpermission", DefaultValue = "[A],[B],[C],[D],[E],[G],[H]", IsRequired = false)]
        public string FullPermission
        {
            get { return (string)this["fullpermission"]; }
            set { this["fullpermission"] = value.ToLowerInvariant(); }
        }

        [ConfigurationProperty(
            "custIdQueryStringParamNameMapping",
            DefaultValue = "custid,CustId,custId,targetCustId,id,cid,recommend,mrecommend,idagent,masterId,MasterId,AgentId,agentId,idAgent,idMaster,ibccustid,ACustID",
            IsRequired = false)]
        public string CustIdQueryStringParamNameMapping
        {
            get { return (string)this["custIdQueryStringParamNameMapping"]; }
            set { this["custIdQueryStringParamNameMapping"] = value; }
        }

        [ConfigurationProperty(
            "custIdsQueryStringParamNameMapping",
            DefaultValue = "SearchAccountIds,custids,CustIds,custIds,targetCustIds,ids,cids,recommends,mrecommends,idagents,masterIds,agentIds,idAgents,idMasters,ibccustids,ACustIDs",
            IsRequired = false)]
        public string CustIdsQueryStringParamNameMapping
        {
            get { return (string)this["custIdsQueryStringParamNameMapping"]; }
            set { this["custIdsQueryStringParamNameMapping"] = value; }
        }

        public string[] CustIdQueryStringParamNames
        {
            get
            {
                if (custIdQueryStringParamNames == null)
                {
#pragma warning disable S2696 // Instance members should not write to "static" fields
                    custIdQueryStringParamNames = CustIdQueryStringParamNameMapping.Split(',');
#pragma warning restore S2696 // Instance members should not write to "static" fields
                }

                return custIdQueryStringParamNames;
            }
        }

        public string[] CustIdsQueryStringParamNames
        {
            get
            {
                if (custIdsQueryStringParamNames == null)
                {
#pragma warning disable S2696 // Instance members should not write to "static" fields
                    custIdsQueryStringParamNames = CustIdsQueryStringParamNameMapping.Split(',');
#pragma warning restore S2696 // Instance members should not write to "static" fields
                }

                return custIdsQueryStringParamNames;
            }
        }

        [ConfigurationProperty("custIdJsonParamNameMapping", IsRequired = false, DefaultValue = "txtCustomerObject,newCustomer")]
        public string CustIdJsonParamNameMapping
        {
            get { return (string)this["custIdJsonParamNameMapping"]; }
            set { this["custIdJsonParamNameMapping"] = value; }
        }

        [ConfigurationProperty("cachedExpireTime", IsRequired = true, DefaultValue = 30)]
        public int CachedExpireTime
        {
            get { return (int)this["cachedExpireTime"]; }
            set { this["cachedExpireTime"] = value; }
        }

        public string[] CustIdJsonParamNames
        {
            get
            {
                if (custIdJsonParamNames == null)
                {
#pragma warning disable S2696 // Instance members should not write to "static" fields
                    custIdJsonParamNames = CustIdJsonParamNameMapping.Split(',');
#pragma warning restore S2696 // Instance members should not write to "static" fields
                }

                return custIdJsonParamNames;
            }
        }

        [ConfigurationProperty(
            "ignoreUrls",
            IsRequired = false,
            DefaultValue = "SignIn.aspx,ProcessAdminLogin.ashx,ProcessAfterLogin.ashx,Captcha.ashx,ChartImg.axd,ReCheckin.ashx,SecurityCode,Header.aspx,Index.aspx")]
        public string IgnoreUrls
        {
            get { return (string)this["ignoreUrls"]; }
            set { this["ignoreUrls"] = value; }
        }

        [ConfigurationProperty("kickOutUrl", IsRequired = false, DefaultValue = "~/default/kickout")]
        public string KickoutUrl
        {
            get { return (string)this["kickOutUrl"]; }
            set { this["kickOutUrl"] = value; }
        }

        [ConfigurationProperty("ignoreRecheckinCustIdList", IsRequired = false, DefaultValue = "68287,68289,1373389,1373580,1374738,4317260,2")]
        public string IgnoreRecheckinCustIdList
        {
            get { return (string)this["ignoreRecheckinCustIdList"]; }
            set { this["ignoreRecheckinCustIdList"] = value; }
        }

        [ConfigurationProperty("enableCheckBelonging", IsRequired = false, DefaultValue = true)]
        public bool EnableCheckBelonging
        {
            get { return (bool)this["enableCheckBelonging"]; }
            set { this["enableCheckBelonging"] = value; }
        }

        public string[] SplitedIgnoreUrls
        {
            get
            {
                if (splitedIgnoreUrls == null)
                {
#pragma warning disable S2696 // Instance members should not write to "static" fields
                    splitedIgnoreUrls = IgnoreUrls.Split(',');
#pragma warning restore S2696 // Instance members should not write to "static" fields
                }

                return splitedIgnoreUrls;
            }
        }

        public IEnumerable<int> IgnoreRecheckinCustIds
        {
            get
            {
                return IgnoreRecheckinCustIdList.Split(',').Select(custId => Convert.ToInt32(custId));
            }
        }
    }
}