namespace Alpha.StaticData.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Mappers;

    public class StaticRepository : DynamicRepository, IDynamicRepository
    {
        public StaticRepository(IRepositoryMapper mapper)
        {
            RepositoryFactory.SetMapper(mapper);
        }

        public override IEnumerable<T> Fetch<T>(ICriteria criteria)
        {
            var repository = RepositoryFactory.Get(criteria);

            return repository == null
                ? base.Fetch<T>(criteria)
                : repository.Fetch<T>(criteria);
        }

        public new async Task<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> FetchMultipleAsync<TFirst, TSecond>(ICriteria criteria)
        {
            var repository = RepositoryFactory.Get(criteria);

            return await (repository == null
                ? base.FetchMultipleAsync<TFirst, TSecond>(criteria)
                : repository.FetchMultipleAsync<TFirst, TSecond>(criteria));
        }
    }
}
