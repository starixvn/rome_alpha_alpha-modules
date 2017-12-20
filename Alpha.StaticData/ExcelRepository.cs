namespace Alpha.StaticData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;

    public abstract class ExcelDynamicRepository : DynamicRepository, IDynamicRepository
    {
        public IEnumerable<T> Fetch<T>(ICriteria criteria)
        {
            var fileName = criteria.GetSettingKey();
            var directory = GetExcelDirectory();
            var excelFile = $"{directory}/{fileName}.xls";

            if (File.Exists(excelFile))
            {
                using (var stream = new FileStream(excelFile, FileMode.Open))
                {
                    return ExcelIO.Read<T>(stream);
                }
            }
            else
            {
                return base.Fetch<T>(criteria);
            }
        }

        public abstract string GetExcelDirectory();
    }
}
