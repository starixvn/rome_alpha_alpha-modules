namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Alpha.SubAccount.Models;

    /// <summary>
    /// Indicate permissions of a sub-account.
    /// </summary>
    [Serializable]
    public class PageAccess : ISerializable, IEntity
    {
        private IDictionary<string, bool> permissions;

        public PageAccess()
        {
            InitPermissions();
        }

        public PageAccess(SerializationInfo info, StreamingContext context) : this()
        {
            foreach (DictionaryEntry accessType in PageAccessType.AccessTypes)
            {
                var key = accessType.Key.ToString();
                permissions[key] = info.GetBoolean(key);
            }
        }

        public bool CreateAccount
        {
            get
            {
                return permissions[PageAccessType.CreateAccount];
            }

            set
            {
                permissions[PageAccessType.CreateAccount] = value;
            }
        }

        public bool UpdateAccount
        {
            get
            {
                return permissions[PageAccessType.UpdateAccount];
            }

            set
            {
                permissions[PageAccessType.UpdateAccount] = value;
            }
        }

        public bool Transfer
        {
            get
            {
                return permissions[PageAccessType.Transfer];
            }

            set
            {
                permissions[PageAccessType.Transfer] = value;
            }
        }

        public bool ViewTotalBets
        {
            get
            {
                return permissions[PageAccessType.ViewTotalBets];
            }

            set
            {
                permissions[PageAccessType.ViewTotalBets] = value;
            }
        }

        public bool ViewReports
        {
            get
            {
                return permissions[PageAccessType.ViewReports];
            }

            set
            {
                permissions[PageAccessType.ViewReports] = value;
            }
        }

        public bool ViewLog
        {
            get
            {
                return permissions[PageAccessType.ViewLog];
            }

            set
            {
                permissions[PageAccessType.ViewLog] = value;
            }
        }

        public bool ViewMemberInfo
        {
            get
            {
                return permissions[PageAccessType.ViewMemberInfo];
            }

            set
            {
                permissions[PageAccessType.ViewMemberInfo] = value;
            }
        }

        public void SetPermission(string key, bool value)
        {
            if (permissions.ContainsKey(key))
            {
                permissions[key] = value;
            }
        }

        public bool GetPermission(string key)
        {
            return permissions.ContainsKey(key) && permissions[key];
        }

        public static implicit operator string(PageAccess value)
        {
            var valuePermissions = value.permissions;

            return string.Join(string.Empty, valuePermissions.Select(kv => kv.Value ? kv.Key : string.Empty));
        }

        public static implicit operator PageAccess(string value)
        {
            var result = new PageAccess();

            if (string.IsNullOrEmpty(value))
            {
                return result;
            }

            foreach (DictionaryEntry accessType in PageAccessType.AccessTypes)
            {
                var key = accessType.Key.ToString();
                result.permissions[key] = value.Contains(key);
            }

            return result;
        }

        public bool IsEmptyPermission()
        {
            return !permissions.Any(kv => kv.Value);
        }

        public bool Contains(PageAccess value)
        {
            return !permissions.Any(kv => kv.Value != value.permissions[kv.Key]);
        }

        public void SetFullPermission()
        {
            foreach (DictionaryEntry accessType in PageAccessType.AccessTypes)
            {
                var key = accessType.Key.ToString();
                permissions[key] = true;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (DictionaryEntry accessType in PageAccessType.AccessTypes)
            {
                var key = accessType.Key.ToString();
                info.AddValue(key, permissions[key]);
            }
        }

        private void InitPermissions()
        {
            permissions = new Dictionary<string, bool>();

            foreach (DictionaryEntry accessType in PageAccessType.AccessTypes)
            {
                permissions.Add(accessType.Key.ToString(), false);
            }
        }
    }
}