namespace Alpha.Announcements.Specs
{
    using Models;

    public class EnableYesterdayTransferSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "9c7da446-b1b0-434b-8965-b157246dbd19";
        private const string SpecificationName = "Enable Yesterday Transfer";

        public EnableYesterdayTransferSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public EnableYesterdayTransferSpecification(IParam param)
            : this()
        {
            if (IsSatisfiedBy(param))
            {
                DoEnableYesterdayTransferBusiness();
            }
        }

        public override bool IsSatisfiedBy(IParam param) => param.EnableYesterdayTransfer;

        private void DoEnableYesterdayTransferBusiness()
        {
            // TODO
            //// Check business and do something
        }
    }
}