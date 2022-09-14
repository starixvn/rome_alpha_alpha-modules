namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public class PageAccessElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public string Id
        {
            get { return (string)this["id"]; }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("permission", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Permission
        {
            get { return (string)this["permission"]; }
            set { this["permission"] = value; }
        }

        [ConfigurationProperty("mainfuncname", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string MainFunctionName
        {
            get { return (string)this["mainfuncname"]; }
            set { this["mainfuncname"] = value; }
        }

        [ConfigurationProperty("subfuncname", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string SubFunctionName
        {
            get { return (string)this["subfuncname"]; }
            set { this["subfuncname"] = value; }
        }

        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("note", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string Note
        {
            get { return (string)this["note"]; }
            set { this["note"] = value; }
        }

        [ConfigurationProperty("ischeckbelonging", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool IsCheckBelonging
        {
            get { return (bool)this["ischeckbelonging"]; }
            set { this["ischeckbelonging"] = value; }
        }

        [ConfigurationProperty("isallowtodobythemselves", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool IsAllowToDoByThemselves
        {
            get { return (bool)this["isallowtodobythemselves"]; }
            set { this["isallowtodobythemselves"] = value; }
        }

        [ConfigurationProperty("isviewedbygroupheadline", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool IsViewedByGroupHeadline
        {
            get { return (bool)this["isviewedbygroupheadline"]; }
            set { this["isviewedbygroupheadline"] = value; }
        }

        [ConfigurationProperty("isallowedcrosslevel", DefaultValue = true, IsRequired = false, IsKey = false)]
        public bool IsAllowedCrossLevel
        {
            get { return (bool)this["isallowedcrosslevel"]; }
            set { this["isallowedcrosslevel"] = value; }
        }

        [ConfigurationProperty("issupportedautofollowbettool", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool IsSupportedAutoFollowBetTool
        {
            get { return (bool)this["issupportedautofollowbettool"]; }
            set { this["issupportedautofollowbettool"] = value; }
        }
    }
}