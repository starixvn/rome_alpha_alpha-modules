using System.Data;

namespace Fanex.Products.Extensions
{
    public static class DataRowExtension
    {
        public static bool HasColumn(this DataRow drow, string columnName)
        {
            return drow.Table.Columns.Contains(columnName);
        }

        public static object GetValue(this DataRow drow, string columnName)
        {
            return drow.HasColumn(columnName) ? drow[columnName] : null;
        }
    }
}
