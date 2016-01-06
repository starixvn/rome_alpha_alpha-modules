namespace Fanex.Razorbill.Security.Configurations.AuthenticationSection
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupLevelCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Collection Type.
        /// </summary>
        /// <value>Collection element Type.</value>
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

        /// <summary>
        /// Index Of function.
        /// </summary>
        /// <param name="authenticationElement">Authentication element.</param>
        /// <returns>Return element.</returns>
        public int IndexOf(GroupLevelElement authenticationElement)
        {
            return BaseIndexOf(authenticationElement);
        }

        /// <summary>
        /// Create New Element.
        /// </summary>
        /// <returns>Return result.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new GroupLevelElement();
        }

        /// <summary>
        /// Get Element Key.
        /// </summary>
        /// <param name="element">Element config.</param>
        /// <returns>Return result.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((GroupLevelElement)element).RoleId;
        }
    }
}