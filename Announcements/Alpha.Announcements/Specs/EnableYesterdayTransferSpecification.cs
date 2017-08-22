namespace Alpha.Announcements.Specs
{
    using Models;

    public class EnableYesterdayTransferSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "9c7da446-b1b0-434b-8965-b157246dbd19";
        private const string SpecificationName = "Enable Yesterday Transfer";
        private bool EnableYesterdayTransfer { get; set; }

        public EnableYesterdayTransferSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public EnableYesterdayTransferSpecification(bool enableYesterdayTransfer)
            : this()
        {
            EnableYesterdayTransfer = enableYesterdayTransfer;
        }

        public override dynamic Value
        {
            get
            {
                return EnableYesterdayTransfer;
            }

            set
            {
                EnableYesterdayTransfer = value ?? false;
            }
        }

        public override bool IsSatisfiedBy(IParam param) => param.EnableYesterdayTransfer.Equals(EnableYesterdayTransfer);
    }
}