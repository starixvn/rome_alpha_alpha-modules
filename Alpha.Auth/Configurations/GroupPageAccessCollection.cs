namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public class GroupPageAccessCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "GroupPageAccess"; }
        }

        public new GroupPageAccessElement this[string path]
        {
            get
            {
                return (GroupPageAccessElement)BaseGet(path);
            }
        }

        public GroupPageAccessElement this[int index]
        {
            get { return (GroupPageAccessElement)BaseGet(index); }
        }

        public int IndexOf(GroupPageAccessElement authenticationElement)
        {
            return BaseIndexOf(authenticationElement);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new GroupPageAccessElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((GroupPageAccessElement)element).RoleId;
        }
    }
}