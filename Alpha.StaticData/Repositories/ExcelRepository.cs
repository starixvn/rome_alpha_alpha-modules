namespace Alpha.StaticData.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Mappers;
    using NPOI.SS.UserModel;
    using NPOIExtensions;

    public abstract class ExcelRepository : DynamicRepository, IDynamicRepository
    {
        protected ExcelRepository()
        {
            InitMapper();
        }

        protected IDataModelMapper Mapper;

        public override IEnumerable<T> Fetch<T>(ICriteria criteria)
        {
            var mappingKey = GetResultModelToExcelFileMappingKey<T>();

            if (!Mapper.Maps.ContainsKey(mappingKey))
            {
                return Enumerable.Empty<T>();
            }

            var template = Mapper.Maps[mappingKey];

            var workbook = LoadWorkbook(template);

            var sheet = workbook?.LoadSheet();

            return sheet?.LoadData(row => Create<T>(row));
        }

        public new virtual async Task<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> FetchMultipleAsync<TFirst, TSecond>(ICriteria criteria)
            => await base.FetchMultipleAsync<TFirst, TSecond>(criteria);

        public virtual string GetResultModelToExcelFileMappingKey<T>()
            => typeof(T).Name;

        public virtual T Create<T>(IRow row) => default(T);

        public abstract void InitMapper();

        public abstract IWorkbook LoadWorkbook(string excelFileName);
    }
}
