namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;
    using Newtonsoft.Json.Linq;

    public class CurrencySpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "9504BDEB-4F6D-4617-BBA6-B1266AC38830";
        private const string SpecificationName = "Currencies";
        private IEnumerable<byte> CurrencyIds { get; set; }

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
                CurrencyIds = value is IEnumerable ? ((JArray)value).Select(x => (byte)x).ToList() : CurrencyIds;
            }
        }

        public override bool IsSatisfiedBy(IParam param) =>
            SpecificationOperator == SpecificationOperator.In
                    ? IsContainsCurrencyId(param.CurrencyId)
                    : !IsContainsCurrencyId(param.CurrencyId);

        private bool IsContainsCurrencyId(byte currencyId) => CurrencyIds.Contains(currencyId);
    }
}