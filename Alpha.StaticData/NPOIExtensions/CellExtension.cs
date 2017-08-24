namespace Alpha.StaticData.NPOIExtensions
{
    using NPOI.SS.UserModel;

    public static class CellExtension
    {
        public static object GetValue(this ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue;

                case CellType.Numeric:
                    return cell.NumericCellValue;

                case CellType.String:
                    return cell.StringCellValue;

                case CellType.Error:
                    return cell.ErrorCellValue;

                case CellType.Formula:
                    return cell.CellFormula;

                default:
                    return string.Empty;
            }
        }
    }
}
