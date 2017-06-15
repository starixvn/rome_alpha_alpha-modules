namespace Alpha.StaticData.NPOIExtensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.POIFS.FileSystem;
    using NPOI.SS.UserModel;

    public static class WorkbookExtension
    {
        public static IWorkbook Load(string filePath)
        {
            using (StreamReader input = new StreamReader(filePath))
            {
                return new HSSFWorkbook(new POIFSFileSystem(input.BaseStream));
            }
        }

        public static ISheet LoadSheet(this IWorkbook workbook, int sheetIndex = 0)
           => workbook.GetSheetAt(sheetIndex);

        public static IEnumerable<T> GetData<T>(this IWorkbook workbook, string sheetName, Func<IRow, T> mapping)
        {
            var sheet = workbook.GetSheet(sheetName);

            return sheet?.LoadData(row => mapping(row));
        }
    }
}
