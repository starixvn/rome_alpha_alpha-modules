namespace Fanex.Razorbill.Security.Configurations
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Razorbill.Security.Configurations.AuthenticationSection;
    using Fanex.Razorbill.Security.Entities;

    public class DeniedPageHelper
    {
        private static Dictionary<int, IEnumerable<DeniedPageElement>> pages;
        private const int AgentRoleId = 2;
        private const int MasterRoleId = 3;
        private const int SuperRoleId = 4;

        public static IEnumerable<DeniedPageElement> GetDeniedPages(int roleId)
        {
            if (pages == null)
            {
                pages = new Dictionary<int, IEnumerable<DeniedPageElement>>
                {
                    { AgentRoleId, DeniedPageForRoleId(AgentRoleId) },
                    { MasterRoleId, DeniedPageForRoleId(MasterRoleId) },
                    { SuperRoleId, DeniedPageForRoleId(SuperRoleId) }
                };
            }

            return pages.ContainsKey(roleId) ? pages[roleId] : Enumerable.Empty<DeniedPageElement>();
        }

        private static IEnumerable<DeniedPageElement> DeniedPageForRoleId(int roleId)
        {
            var level = AuthenticationSection.AuthenticationSection.Instance.GroupLevel.Cast<GroupLevelElement>()
                .FirstOrDefault(i => i.RoleId == roleId);
            if (level != null)
            {
                return level.DeniedPages.Cast<DeniedPageElement>();
            }

            return Enumerable.Empty<DeniedPageElement>();
        }
    }
}
