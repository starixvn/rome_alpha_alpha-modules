using System.Collections;
using System.Data;
using Fanex.Products.Enums;

namespace Fanex.Products.Services
{
    public interface IProduct
    {
        bool Active { get; }

        bool Disabled { get; }

        bool? Synced { get; }

        bool ShowReport { get; }

        bool ShowSettings { get; }

        void SetProductStatusToSession(Hashtable outputParams);

        DCSIcon GetDCSIcon(DataRow drow);

        bool ShowDCSLinkForDownline(DataRow drow, int loggedInUserRole, int downlineRoleId);

        bool ShowEnableStatusLinkForDownline(DataRow drow);
    }
}
