using System;
using System.Collections;
using System.Data;
using Fanex.Products.Enums;
using Fanex.Products.Extensions;

namespace Fanex.Products.Services
{
    public class Product_23 : BaseProduct, IProduct
    {
        public Product_23(bool webConfigActive)
            : base(webConfigActive)
        {
        }

        public override bool? Synced
        {
            get
            {
                return GetSynced();
            }
        }

        public override bool ShowSettings
        {
            get
            {
                return base.ShowSettings && GetSynced() == true;
            }
        }

        public override void SetProductStatusToSession(Hashtable outputParams)
        {
            base.SetProductStatusToSession(outputParams);

            string lowerName = Name.ToLowerInvariant();
            string storedProcedureName = "synced" + lowerName;

            if (outputParams.ContainsKey(storedProcedureName))
            {
                SetSynced(Convert.ToBoolean(outputParams[storedProcedureName]));
            }
        }

        public override DCSIcon GetDCSIcon(DataRow drow)
        {
            if (ShowSettings)
            {
                string lowerName = Name.ToLowerInvariant().NormalizeString();

                string syncedColumnName = "synced" + lowerName;
                object synced = drow.GetValue(syncedColumnName);

                if (synced == null || !Convert.ToBoolean(synced))
                {
                    return DCSIcon.Synchronized;
                }

                string disabledColumnName = "disable" + lowerName;
                object disabled = drow.GetValue(disabledColumnName);

                if (disabled != null && !Convert.ToBoolean(disabled))
                {
                    return DCSIcon.EnabledEdit;
                }
            }

            return DCSIcon.DisabledEdit;
        }

        public override bool ShowDCSLinkForDownline(DataRow drow, int loggedInUserRole, int downlineRoleId)
        {
            if (!string.IsNullOrEmpty(CustomerSettingLink) && ShowSettings)
            {
                // Direct downline or Member.
                return (downlineRoleId + 1 == loggedInUserRole) || downlineRoleId == 1;
            }

            return false;
        }

        public override bool ShowEnableStatusLinkForDownline(DataRow drow)
        {
            if (!string.IsNullOrEmpty(EnableStatusLink) && ShowSettings)
            {
                object synced = drow.GetValue("IsSync");

                return synced != null && Convert.ToBoolean(synced);
            }

            return false;
        }
    }
}
