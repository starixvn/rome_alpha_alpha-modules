namespace Alpha.Announcements.Specs
{
    using Models;

    public class SyncedRnGCasinoSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "ede728e2-f48b-4f2c-80fd-1ff5954b3bdb";
        private const string SpecificationName = "Synced RnG Casino";
        private bool SyncedRnGCasino { get; set; }

        public SyncedRnGCasinoSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public SyncedRnGCasinoSpecification(bool syncedRnGCasino)
            : this()
        {
            SyncedRnGCasino = syncedRnGCasino;
        }

        public override dynamic Value
        {
            get
            {
                return SyncedRnGCasino;
            }

            set
            {
                SyncedRnGCasino = value ?? false;
            }
        }

        public override bool IsSatisfiedBy(IParam param) => param.IsSyncedCasino.Equals(SyncedRnGCasino);
    }
}