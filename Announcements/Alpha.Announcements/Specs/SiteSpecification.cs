namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class SiteSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "6edc73e0-aa87-4757-96e7-e5ffea0ce06a";
        private const string SpecificationName = "Sites";

        public SiteSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public SiteSpecification(IEnumerable<string> sites)
            : this()
        {
            Sites = sites;
        }

        public override dynamic Value
        {
            get
            {
                return Sites;
            }

            set
            {
                Sites = value is IEnumerable<string> ? value : Sites;
            }
        }

        public IEnumerable<string> Sites { get; set; }

        public override bool IsSatisfiedBy(IParam user) =>
            SpecificationOperator == SpecificationOperator.In
                    ? IsContainsSite(user.Site)
                    : !IsContainsSite(user.Site);

        private bool IsContainsSite(string site) => Sites.Contains(site);
    }
}