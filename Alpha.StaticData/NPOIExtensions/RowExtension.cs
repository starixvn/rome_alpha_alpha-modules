namespace Alpha.StaticData.Npoi
{
    using Fanex.Common;
    using NPOI.SS.UserModel;
    using NPOIExtensions;

    public static class RowExtension
    {
        public static T Get<T>(this IRow row, object columnIndex)
        {
            var cell = row.GetCell((int)columnIndex);
            var value = cell?.GetValue();

            if (value == null && typeof(T) == typeof(string))
            {
                value = string.Empty;
            }

            return Converter.To<T>(value);
        }
    }
}
