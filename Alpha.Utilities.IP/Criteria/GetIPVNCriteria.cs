namespace Alpha.Utilities.IP.Criteria
{
    using Fanex.Data.Repository;

    public class GetIPVNCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "GetIPVN";

        public override bool IsValid() => true;
    }
}