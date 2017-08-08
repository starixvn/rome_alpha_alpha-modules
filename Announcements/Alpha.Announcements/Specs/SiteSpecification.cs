namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;
    using Newtonsoft.Json.Linq;
    using System;

    public class SiteSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "6edc73e0-aa87-4757-96e7-e5ffea0ce06a";
        private const string SpecificationName = "Site Names";
        private IEnumerable<string> Sites { get; set; }

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
                Sites = value is IEnumerable ? ((JArray)value).Select(x => x.ToString()).ToList() : Sites;
            }
        }

        public override bool IsSatisfiedBy(IParam param) =>
            SpecificationOperator == SpecificationOperator.In
                    ? IsContainsSite(param.Site)
                    : !IsContainsSite(param.Site);

        private bool IsContainsSite(string site) => Sites.Any(i => i.Equals(site, StringComparison.OrdinalIgnoreCase));
    }
}