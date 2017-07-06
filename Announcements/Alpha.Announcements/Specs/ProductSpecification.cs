namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using System.Collections;

    public class ProductSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "e4fe1443-c8d0-4e63-a355-08661859295e";
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