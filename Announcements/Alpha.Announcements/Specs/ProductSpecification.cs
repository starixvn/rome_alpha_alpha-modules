namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using System.Collections;

    public class ProductSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "E4FE1443-C8D0-4E63-A355-08661859295E";
        private const string SpecificationName = "Products";
        private IEnumerable<byte> ProductIds { get; set; }

        public ProductSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public ProductSpecification(IEnumerable<byte> productIds)
            : this()
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
                ProductIds = value is IEnumerable ? ((JArray)value).Select(x => (byte)x).ToList() : ProductIds;
            }
        }

        public override bool IsSatisfiedBy(IParam param) =>
            SpecificationOperator == SpecificationOperator.In
                    ? IsContainsOfferedProductId(param.OfferedProductIds)
                    : !IsContainsOfferedProductId(param.OfferedProductIds);

        private bool IsContainsOfferedProductId(IEnumerable<byte> offeredProductIds)
             => ProductIds.Any(productId => offeredProductIds.Any(offeredProductId => offeredProductId.Equals(productId)));
    }
}