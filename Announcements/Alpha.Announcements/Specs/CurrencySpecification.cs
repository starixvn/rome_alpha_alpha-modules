namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class CurrencySpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "9504BDEB-4F6D-4617-BBA6-B1266AC38830";
        private const string SpecificationName = "Currencies";

        public CurrencySpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public CurrencySpecification(IEnumerable<byte> currencyIds)
            : this()
        {
            CurrencyIds = currencyIds;
        }

        public override dynamic Value
        {
            get
            {
                return CurrencyIds;
            }

            set
            {
                CurrencyIds = value is IEnumerable<byte> ? value : CurrencyIds;
            }
        }

        public IEnumerable<byte> CurrencyIds { get; set; }

        public override bool IsSatisfiedBy(IParam user) =>
            SpecificationOperator == SpecificationOperator.In
                    ? IsContainsCurrencyId(user.CurrencyId)
                    : !IsContainsCurrencyId(user.CurrencyId);

        private bool IsContainsCurrencyId(byte currencyId) => CurrencyIds.Contains(currencyId);
    }
}