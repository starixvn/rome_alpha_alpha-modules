namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public class GroupLevelCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "GroupLevel"; }
        }

        public new GroupLevelElement this[string path]
        {
            get
            {
                return (GroupLevelElement)BaseGet(path);
            }
        }

        public GroupLevelElement this[int index]
        {
            get { return (GroupLevelElement)BaseGet(index); }
        }

        public int IndexOf(GroupLevelElement authenticationElement)
        {
            return BaseIndexOf(authenticationElement);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new GroupLevelElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((GroupLevelElement)element).RoleId;
        }
    }
}