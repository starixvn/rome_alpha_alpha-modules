namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "E4FE1443-C8D0-4E63-A355-08661859295E";
        private const string SpecificationName = "Products";

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
                ProductIds = value is IEnumerable<byte> ? value : ProductIds;
            }
        }

        public IEnumerable<byte> ProductIds { get; set; }

        public override bool IsSatisfiedBy(IParam user) =>
            SpecificationOperator == SpecificationOperator.In
                    ? IsContainsOfferedProductId(user.OfferedProductIds)
                    : !IsContainsOfferedProductId(user.OfferedProductIds);

        private bool IsContainsOfferedProductId(IEnumerable<byte> offeredProductIds)
             => ProductIds.Any(productId => offeredProductIds.Any(offeredProductId => offeredProductId.Equals(productId)));
    }
}