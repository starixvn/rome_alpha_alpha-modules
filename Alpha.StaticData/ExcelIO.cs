namespace Alpha.StaticData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;

    using NPOI.SS.UserModel;

    public static class ExcelIO
    {
        public static IEnumerable<T> Read<T>(Stream file)
        {
            var reader = new ExcelReader(file);

            return Read<T>(reader);
        }

        private static IEnumerable<T> Read<T>(this ExcelReader reader)
            => GetResult<T>(reader, reader.Sheet);

        private static IEnumerable<T> GetResult<T>(ExcelReader reader, ISheet sheet)
        {
            var mapper = GetFunc<T>(reader);

            for (var i = reader.StartRow; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (IsEmptyRow(row))
                {
                    continue;
                }

                yield return mapper(row);
            }
        }

        private static Func<IRow, T> GetFunc<T>(ExcelReader reader)
        {
            Expression<Func<string, IRow, Type, object>> getPropValueFunc = (column, row, t) => reader.GetPropValue(column, row, t);
            var rowArg = Expression.Parameter(typeof(IRow));

            var body = CreateConstructorParameter(rowArg, getPropValueFunc, typeof(T));

            return Expression.Lambda<Func<IRow, T>>(body, rowArg).Compile();            
        }

        private static Expression CreateConstructorParameter(Expression row, Expression lambda, Type type)
        {
            var ctor = type.GetConstructors().First(i => i.GetParameters().Length > 0);
            var arrayAccess =
                ctor.GetParameters()
                .Select((x, i)
                => Expression.Convert(
                        Expression.Invoke(lambda, Expression.Constant(x.Name), row, Expression.Constant(x.ParameterType)),
                    x.ParameterType));

            return Expression.New(ctor, arrayAccess);
        }       

        private static bool IsEmptyRow(IRow row)
        {
            return
                row == null ||
                row.Cells.Count == 0 ||
                row.Cells.TrueForAll(c => c.CellType == CellType.Blank || c.CellType == CellType.String && string.IsNullOrEmpty(c.StringCellValue));
        }
    }
}
