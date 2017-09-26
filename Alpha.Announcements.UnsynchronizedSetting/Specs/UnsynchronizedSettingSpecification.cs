namespace Alpha.Announcements.UnsynchronizedSetting.Specs
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Specs;
    using Models;
    using Newtonsoft.Json.Linq;

    public class UnsynchronizedSettingSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "6958e609-fd9c-4758-b757-e74e0466afd4";
        private const string SpecificationName = "Unsynchronized Setting";

        public IEnumerable<int> ProductIds { get; set; }
        
        public UnsynchronizedSettingSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public UnsynchronizedSettingSpecification(IEnumerable<int> productIds) : this()
        {
            ProductIds = productIds;
        }

        public override dynamic Value
        {
            get
            {
                return ProductIds;
            }

            set
            {
                ProductIds = value is IEnumerable ? ((JArray)value).Select(x => (int)x).ToList() : ProductIds;
            }
        }

        public override bool IsSatisfiedBy(IParam param) => ProductIds.Any();
    }
}
