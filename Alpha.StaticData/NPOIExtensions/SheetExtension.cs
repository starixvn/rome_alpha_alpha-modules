namespace Alpha.StaticData.NPOIExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NPOI.SS.UserModel;

    public static class SheetExtension
    {
        public static IEnumerable<T> LoadData<T>(this ISheet sheet, Func<IRow, T> mapping)
        {
            if (sheet == null)
            {
                return Enumerable.Empty<T>();
            }

            var items = new List<T>();

            sheet.RemoveRow(sheet.GetRow(0));

            foreach (IRow row in sheet)
            {
                if (row.Cells.Any())
                {
                    items.Add(mapping(row));
                }
            }

            return items;
        }
    }
}
