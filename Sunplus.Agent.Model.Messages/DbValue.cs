namespace Sunplus.Agent.Model.DL.UserMessage
{
    using System;
    using System.Data;

    [Obsolete("Should rename to Sunplus.Agent.Model.Messages")]
    internal class DbValue<T>
    {
        public static void GetValue(DataRow row, string fieldName, ref T output)
        {
            if (IsNotNull(row, fieldName))
            {
                output = (T)row[fieldName];
            }
        }

        public static void GetValue(DataRow row, string fieldName, T output)
        {
            if (IsNotNull(row, fieldName))
            {
                output = (T)row[fieldName];
            }
        }

        public static T GetValue(DataRow row, string fieldName)
        {
            if (IsNotNull(row, fieldName))
            {
                return (T)row[fieldName];
            }

            return default(T);
        }

        private static bool IsNotNull(DataRow row, string fieldName)
        {
            return row.Table.Columns.Contains(fieldName) && (!row.IsNull(fieldName));
        }
    }
}
