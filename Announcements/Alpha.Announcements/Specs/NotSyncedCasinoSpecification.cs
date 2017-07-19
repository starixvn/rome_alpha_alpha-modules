namespace Alpha.Announcements.Specs
{
    using Models;

    public class NotSyncedCasinoSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "ede728e2-f48b-4f2c-80fd-1ff5954b3bdb";
        private const string SpecificationName = "Not Synced Casino";

        public NotSyncedCasinoSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public NotSyncedCasinoSpecification(IParam param)
            : this()
        {
            if(IsSatisfiedBy(param))
            {
                DoNotSyncedCasinoBusiness();
            }
        }

        public override bool IsSatisfiedBy(IParam param) => !param.IsSyncedCasino;

        private void DoNotSyncedCasinoBusiness()
        {
            // TODO
            //// Check business and do something
        }
    }
}