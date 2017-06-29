namespace Alpha.Announcements.Services
{
    using Models;
    using Specs;
    using System.Collections.Generic;
    using System.Linq;

    public interface SpecificationService
    {
        bool IsSatisfied(IEnumerable<ISpecification<IParam>> specifications, IParam param);
    }

    public class SpecificationServiceImpl : SpecificationService
    {
        public bool IsSatisfied(IEnumerable<ISpecification<IParam>> specifications, IParam param)
        {
            if (param == null)
            {
                return false;
            }

            if (specifications == null || !specifications.Any())
            {
                return true;
            }

            var accumulateSpecification = specifications.First();
            var totalSpec = specifications.Count();
            for (int i = 1; i < totalSpec; i++)
            {
                var nextSpec = specifications.ElementAt(i);
                accumulateSpecification = nextSpec.ConjunctionOperator == ConjunctionOperator.Or
                       ? accumulateSpecification.Or(nextSpec)
                       : accumulateSpecification.And(nextSpec);
            }

            return accumulateSpecification.IsSatisfiedBy(param);
        }
    }
}