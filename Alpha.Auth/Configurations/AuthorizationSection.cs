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

        [ConfigurationProperty("fullpermission", DefaultValue = "[A],[B],[C],[D],[E],[G],[H]", IsRequired = false)]
        public string FullPermission
        {
            get { return (string)this["fullpermission"]; }
            set { this["fullpermission"] = value.ToLower(); }
        }

        [ConfigurationProperty(
            "custIdQueryStringParamNameMapping",
            DefaultValue = "custid,CustId,custId,targetCustId,id,cid,recommend,mrecommend,idagent,masterId,agentId,idAgent,idMaster,ibccustid,ACustID",
            IsRequired = false)]
        public string CustIdQueryStringParamNameMapping
        {
            get { return (string)this["custIdQueryStringParamNameMapping"]; }
            set { this["custIdQueryStringParamNameMapping"] = value; }
        }

        public string[] CustIdQueryStringParamNames
        {
            get
            {
                return CustIdQueryStringParamNameMapping.Split(',');
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
                return CustIdJsonParamNameMapping.Split(',');
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
                return IgnoreUrls.Split(',');
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