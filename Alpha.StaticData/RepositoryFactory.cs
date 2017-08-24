using System;
using Alpha.StaticData.Mappers;
using Alpha.StaticData.Repositories;
using Fanex.Data.Repository;

namespace Alpha.StaticData
{
    public static class RepositoryFactory
    {
        private static IRepositoryMapper mapper;

        public static void SetMapper(IRepositoryMapper repositoryMapper)
        {
            mapper = repositoryMapper;
        }

        public static ExcelRepository Get(ICriteria criteria)
        {
            foreach (var mapping in mapper.Maps)
            {
                var isType = IsType(criteria.GetType(), mapping.Key);

                if (isType)
                {
                    return (ExcelRepository)Activator.CreateInstance(mapping.Value);
                }
            }

            return null;
        }

        private static bool IsType(Type criteriaType, string expectedType)
        {
            if (IsDirrectType(criteriaType, expectedType))
            {
                return true;
            }

            var baseType = criteriaType.BaseType;

            while (baseType != null && !IsDirrectType(baseType, expectedType))
            {
                baseType = baseType.BaseType;
            }

            return baseType != null;
        }

        private static bool IsDirrectType(Type criteriaType, string expectedType)
            => string.Equals(criteriaType.Name, expectedType, StringComparison.Ordinal)
            || string.Equals(criteriaType.FullName, expectedType, StringComparison.Ordinal);
    }
}
