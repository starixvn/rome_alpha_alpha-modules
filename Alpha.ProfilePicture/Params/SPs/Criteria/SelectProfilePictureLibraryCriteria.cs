namespace Alpha.ProfilePicture.Params.SPs.Criteria
{
    using Fanex.Data.Repository;

    public class SelectProfilePictureLibraryCriteria : CriteriaBase
    {
        public SelectProfilePictureLibraryCriteria()
        {
        }

        public override string GetSettingKey() => "Age_CustomerProfilePictureLibrarySel";

        public byte Mode => 1;

        public override bool IsValid() => true;
    }
}
