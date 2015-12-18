using System;
using System.Collections.Generic;
using System.Diagnostics;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Sunplus.Agent.NPOI
{
    public static class HSSFSheetExtensions
    {
        public static void CopyRows(this ISheet sheet, ISheet sourceSheet, int sourceRowNum, int destinateRowNum, int total)
        {
            if (total <= 0) return;
            for (var i = 0; i < total; i++)
            {
                sheet.CopyRow(sourceSheet, sourceRowNum + i, destinateRowNum + i);
            }
        }

        public static void CopyRow(this ISheet sheet, ISheet sourceSheet, int sourceRowNum, int destinateRowNum, int firstColumn, int lastColumn)
        {
            if (sheet.LastRowNum < destinateRowNum)
            {
                for (var i = sheet.LastRowNum + 1; i <= destinateRowNum; i++)
                {
                    sheet.CreateRow(i);
                }
            }

            // Get the source / new row            
            var sourceRow = sourceSheet.GetRow(sourceRowNum);
            if (sourceRow == null) return;

            var newRow = sheet.GetRow(destinateRowNum);
            if (newRow == null) newRow = sheet.CreateRow(destinateRowNum);
            newRow.Height = sourceRow.Height;

            // Loop through source columns to add to new row
            int firstCellNum = Math.Min(sourceRow.FirstCellNum, firstColumn);
            int lastCellNum = Math.Min(sourceRow.LastCellNum, lastColumn + 1);
            for (int i = firstCellNum; i < lastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                var oldCell = sourceRow.GetCell(i);
                // If the old cell is null jump to next cell
                if (oldCell == null)
                    continue;

                var newCell = newRow.CreateCell(i);
                CloneCell(newCell, oldCell);

                // copy Merged
                CellRangeAddress sourceRange = null;
                for (var j = 0; j < sourceSheet.NumMergedRegions; j++)
                {
                    CellRangeAddress range = sourceSheet.GetMergedRegion(j);
                    if (range.FirstColumn == oldCell.ColumnIndex
                        && range.FirstRow == oldCell.RowIndex)
                    {
                        sourceRange = range;
                        break;
                    }
                }

                if (sourceRange != null)
                {
                    CellRangeAddress newRange = new CellRangeAddress(newCell.RowIndex, newCell.RowIndex + sourceRange.LastRow - sourceRange.FirstRow,
                        newCell.ColumnIndex, newCell.ColumnIndex + sourceRange.LastColumn - sourceRange.FirstColumn);
                    sheet.AddMergedRegion(newRange);
                }
            }
        }

        public static void CopyRow(this ISheet sheet, ISheet sourceSheet, int sourceRowNum, int destinateRowNum)
        {
            sheet.CopyRow(sourceSheet, sourceRowNum, destinateRowNum, 0, Int32.MaxValue - 1);
        }

        public static void RemoveRow(this ISheet sheet, int rowIndex)
        {
            sheet.RemoveRow(sheet.GetRow(rowIndex));
        }

        public static void RemoveColumns(this ISheet sheet, List<int> columns)
        {
            columns.Sort();
            for (int i = columns.Count - 1; i >= 0; i--)
            {
                RemoveColumn(sheet, columns[i]);
            }
        }

        public static void RemoveColumn(this ISheet sheet, int columnIndex)
        {
            int maxColumn = 0;
            bool adjustColumn = true;
            for (int r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++)
            {
                var row = sheet.GetRow(r);

                // if no row exists here; then nothing to do; next!
                if (row == null)
                    continue;

                // if the row doesn't have this many columns then we are good; next!
                int lastColumn = row.LastCellNum;
                maxColumn = Math.Max(maxColumn, lastColumn);

                if (lastColumn < columnIndex)
                    continue;

                for (int x = columnIndex; x < lastColumn; x++)
                {
                    var oldCell = row.GetCell(x);
                    if (oldCell != null)
                        row.RemoveCell(oldCell);

                    if (x < lastColumn)
                    {
                        // move cell
                        var nextCell = row.GetCell(x + 1);
                        if (nextCell != null)
                        {
                            var newCell = row.CreateCell(x, nextCell.CellType);
                            if (adjustColumn)
                            {
                                sheet.SetColumnWidth(newCell.ColumnIndex, sheet.GetColumnWidth(nextCell.ColumnIndex));
                            }
                            CloneCell(newCell, nextCell);
                        }
                    }
                    else
                    {
                        var removeCell = row.GetCell(x);
                        if (removeCell != null)
                            row.RemoveCell(removeCell);
                    }
                }

                // adjust merged region
                for (int i = sheet.NumMergedRegions - 1; i >= 0; i--)
                {
                    var mergedRegion = sheet.GetMergedRegion(i);
                    //if (r >= mergedRegion.FirstRow && r <= mergedRegion.LastRow)
                    if (r == mergedRegion.FirstRow)
                    {
                        if (columnIndex == mergedRegion.FirstColumn && columnIndex == mergedRegion.LastColumn)
                        {
                            sheet.RemoveMergedRegion(i);
                        }
                        else if (mergedRegion.FirstColumn > columnIndex)
                        {
                            mergedRegion.FirstColumn = mergedRegion.FirstColumn - 1;
                            mergedRegion.LastColumn = mergedRegion.LastColumn - 1;
                        }
                        else if (columnIndex <= mergedRegion.LastColumn)
                        {
                            mergedRegion.LastColumn = mergedRegion.LastColumn - 1;
                        }
                    }
                }

                adjustColumn = false;
            }

            //// Adjust the column widths
            //for (int c = 0; c < maxColumn; c++)
            //{
            //    sheet.SetColumnWidth(c, sheet.GetColumnWidth(c + 1));
            //}
        }

        public static void CloneCell(this ISheet sheet, ICell cNew, ICell cOld)
        {
            CloneCell(cNew, cOld);
        }

        /*
         * Takes an existing Cell and merges all the styles and forumla into the new one
         */
        private static void CloneCell(ICell newCell, ICell oldCell)
        {
            if (oldCell.CellComment != null)
                newCell.CellComment = oldCell.CellComment;
            if (oldCell.Hyperlink != null)
                newCell.Hyperlink = oldCell.Hyperlink;
            if (oldCell.CellStyle != null)
                newCell.CellStyle = oldCell.CellStyle;
            newCell.SetCellType(oldCell.CellType);

            switch (newCell.CellType)
            {
                case CellType.Boolean:
                    {
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;
                    }
                case CellType.Numeric:
                    {
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;
                    }
                case CellType.String:
                    {
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                    }
                case CellType.Error:
                    {
                        newCell.SetCellValue(oldCell.ErrorCellValue);
                        break;
                    }
                case CellType.Formula:
                    {
                        newCell.CellFormula = oldCell.CellFormula;
                        break;
                    }
            }

        }

        public static ICell GetCell(this ISheet sheet, int rowIndex, int colIndex)
        {
            return sheet.GetRow(rowIndex).GetCell(colIndex);
        }

        public static int MergeCells(this ISheet sheet, int firstRow, int firstCol, int lastRow, int lastCol)
        {
            var rangeAddress = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            return sheet.AddMergedRegion(rangeAddress);
        }

        public static void RemoveMergedRegions(this ISheet sheet, int firstRow, int firstCol, int lastRow, int lastCol)
        {
            for (int i = sheet.NumMergedRegions - 1; i >= 0; i--)
            {
                var mergedRegion = sheet.GetMergedRegion(i);
                if (mergedRegion.FirstRow >= firstRow && mergedRegion.LastRow <= lastRow &&
                    mergedRegion.FirstColumn >= firstCol && mergedRegion.LastColumn <= lastCol)
                {
                    sheet.RemoveMergedRegion(i);
                }
            }
        }

        public static ICell CreateCell(this ISheet sheet, int rowId, int colId, int rowSpan, int colSpan)
        {
            Debug.Assert(rowSpan >= 0, "rowSpan must be more than or equal 0");
            Debug.Assert(colSpan >= 0, "colSpan must be more than or equal 0");

            ICell cell = sheet.CreateCell(rowId, colId);
            if (rowSpan == 0 && colSpan == 0) return cell;
            MergeCells(sheet, rowId, colId, rowId + rowSpan, colId + colSpan);
            return cell;
        }

        public static ICell CreateCell(this ISheet sheet, int rowId, int colId, int rowSpan, int colSpan, ICellStyle cellStyle)
        {
            ICell cell = sheet.CreateCell(rowId, colId, rowSpan, colSpan);
            cell.CellStyle = cellStyle;
            return cell;
        }

        public static ICell CreateCell(this ISheet sheet, int rowId, int colId)
        {
            IRow row = sheet.GetRow(rowId);
            if (row == null) row = sheet.CreateRow(rowId);

            ICell cell = row.GetCell(colId);
            if (cell == null) cell = row.CreateCell(colId);

            return cell;
        }
    }

    public static class WorkbookExtensions
    {
        public static ISheet CloneSheet(this IWorkbook workbook, string sourceSheetName, string newSheetName)
        {
            var sheetIndex = workbook.GetSheetIndex(sourceSheetName);
            var newSheet = workbook.CloneSheet(sheetIndex);
            var newSheetIndex = workbook.GetSheetIndex(newSheet);
            workbook.SetSheetName(newSheetIndex, newSheetName);
            return newSheet;
        }
    }

    public static class RowExtensions
    {
        public static ICell CreateCell(this IRow row, int colId, ICellStyle cellStyle)
        {
            ICell cell = row.CreateCell(colId);
            cell.CellStyle = cellStyle;
            return cell;
        }

        public static void WriteCells(this IRow row, int colId, params object[] values)
        {
            int id = colId;
            if (values.Length == 1 && values[0] is List<object>)
                values = ((List<object>)values[0]).ToArray();

            foreach (var value in values)
            {
                row.GetCell(id).SetCellValue(value);
                id++;
            }
        }
    }

    public static class CellRangeAddressExtensions
    {
        public static CellRangeAddress Clone(this CellRangeAddress range)
        {
            CellRangeAddress result = new CellRangeAddress(range.FirstRow, range.LastRow, range.FirstColumn, range.LastColumn);
            return result;
        }
    }

    public static class CellExtensions
    {
        public static void SetCellValue(this ICell cell, object value)
        {
            if (value == null)
                cell.SetCellValue("0");
            else
            {
                if (value is string)
                    cell.SetCellValue((string)value);
                else if (value is int)
                    cell.SetCellValue((int)value);
                else if (value is long)
                    cell.SetCellValue((long)value);
                else if (value is double)
                    cell.SetCellValue((double)value);
                else if (value is decimal)
                    cell.SetCellValue((decimal)value);
                else if (value is bool)
                    cell.SetCellValue((bool)value);
                else if (value is System.DateTime)
                    cell.SetCellValue((System.DateTime)value);
                else if (value is IRichTextString)
                    cell.SetCellValue((IRichTextString)value);
                else
                    cell.SetCellValue("Undefined");
            }
        }

        public static void SetCellValue(this ICell cell, double? value)
        {
            if (value == null) value = 0;
            cell.SetCellValue((double)value);
        }

        public static void SetCellValue(this ICell cell, decimal value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        public static void SetCellValue(this ICell cell, decimal? value)
        {
            if (value == null) value = 0;
            cell.SetCellValue((decimal)value);
        }

        public static void SetCellValue(this ICell cell, int value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        public static void SetCellValue(this ICell cell, int? value)
        {
            if (value == null) value = 0;
            cell.SetCellValue((int)value);
        }

        public static void SetCellValue(this ICell cell, long value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        public static void SetCellValue(this ICell cell, long? value)
        {
            if (value == null) value = 0;
            cell.SetCellValue((long)value);
        }

        public static void CopyFrom(this ICell cell, ICell sourceCell)
        {
            // copy Merged
            CellRangeAddress sourceRange = null;
            for (var i = 0; i < sourceCell.Sheet.NumMergedRegions; i++)
            {
                CellRangeAddress range = sourceCell.Sheet.GetMergedRegion(i);
                if (range.FirstColumn <= sourceCell.ColumnIndex
                    && range.LastColumn >= sourceCell.ColumnIndex
                    && range.FirstRow <= sourceCell.RowIndex
                    && range.LastRow >= sourceCell.RowIndex)
                {
                    sourceRange = range;
                    break;
                }
            }

            if (sourceRange != null)
            {
                cell.Sheet.AddMergedRegion(sourceRange.Clone());
            }

            // copy Style
            cell.CellStyle = sourceCell.CellStyle;
        }
    }
}
