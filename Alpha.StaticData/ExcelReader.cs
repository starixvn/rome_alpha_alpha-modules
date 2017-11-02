namespace Alpha.StaticData
{
    using System;
    using System.IO;
    using System.Linq;
    using NPOI.SS.UserModel;

    public class ExcelReader
    {
        private static DataFormatter formatter = new DataFormatter();

        public ExcelReader(Stream file)
        {
            Sheet = WorkbookFactory.Create(file).GetSheetAt(0);
            Headers = GetHeaders();
        }

        public ISheet Sheet { get; }

        public int StartRow => 1;

        public int HeaderRow => 0;

        public ILookup<string, int> Headers { get; }

        public object GetPropValue(string column, IRow row, Type rowType)
        {
            var values = Headers[column.ToLower()].Select(i => CellReader(row, i, rowType));

            return values.First();
        }

        private object CellReader(IRow row, int index, Type t)
        {
            var cell = row.GetCell(index);

            if (t == typeof(int))
            {
                return ReadInt(cell);
            }

            if (t == typeof(DateTime))
            {
                return ReadDateTime(cell);
            }

            if (t == typeof(byte))
            {
                return ReadByte(cell);
            }

            if (t == typeof(bool))
            {
                return ReadBool(cell);
            }

            if (t == typeof(decimal))
            {
                return ReadDecimal(cell);
            }

            return ReadString(cell);
        }

        public object ReadDecimal(ICell cell)
        {
            if (cell == null)
            {
                return default(int);
            }

            return
                cell.CellType == CellType.String ?
                decimal.Parse(cell.StringCellValue) :
                (decimal)cell.NumericCellValue;
        }

        public object ReadByte(ICell cell)
        {
            if (cell == null)
            {
                return default(byte);
            }

            return
                cell.CellType == CellType.String ?
                byte.Parse(cell.StringCellValue) :
                (byte)cell.NumericCellValue;
        }

        public object ReadInt(ICell cell)
        {
            if (cell == null)
            {
                return default(int);
            }

            return
                cell.CellType == CellType.String ?
                int.Parse(cell.StringCellValue) :
                (int)cell.NumericCellValue;
        }

        public object ReadBool(ICell cell)
        {
            if (cell == null)
            {
                return default(bool);
            }

            var value =
                cell.CellType == CellType.String ?
                cell.StringCellValue :
                cell.NumericCellValue.ToString();

            return value == "1";
        }

        public object ReadDouble(ICell cell)
        {
            if (cell == null)
            {
                return default(double);
            }

            return
                cell.CellType == CellType.String ?
                double.Parse(cell.StringCellValue) :
                cell.NumericCellValue;
        }

        public object ReadString(ICell cell)
        {
            var value = formatter.FormatCellValue(cell);

            return value ?? string.Empty;
        }

        public object ReadDateTime(ICell cell)
        {
            return cell.DateCellValue;
        }

        private ILookup<string, int> GetHeaders()
        {
            return
                Sheet
                .GetRow(HeaderRow)
                .Cells
                .ToLookup(cell => cell.StringCellValue.ToLower(), cell => cell.ColumnIndex);
        }
    }
}
