namespace Sunplus.Agent.Model.Entities
{
    using System.Collections;

    public struct PageAccessType
    {
        public const string CreateAccount = "[A]";
        public const string UpdateAccount = "[B]";
        public const string Transfer = "[C]";
        public const string ViewTotalBets = "[D]";
        public const string ViewReports = "[E]";
        public const string ViewLog = "[G]";
        public const string ViewMemberInfo = "[H]";
        public const string ViewBetLists = "[L]";

        private static Hashtable accessTypes = new Hashtable
        {
            [CreateAccount] = false,
            [UpdateAccount] = false,
            [Transfer] = false,
            [ViewTotalBets] = false,
            [ViewReports] = false,
            [ViewLog] = false,
            [ViewMemberInfo] = false,
            [ViewBetLists] = false
        };

        public static Hashtable AccessTypes
        {
            get
            {
                return accessTypes;
            }
        }

        public static bool IsValidType(string type)
        {
            return accessTypes.ContainsKey(type);
        }
    }
}