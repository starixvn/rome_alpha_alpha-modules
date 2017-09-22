namespace Alpha.Announcements.UnsynchronizedSetting.Criteria
{
    using Fanex.Data.Repository;

    public class UnsynchronizedSettingCriteria : CriteriaBase
    {
        public UnsynchronizedSettingCriteria(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }

        public override string GetSettingKey() => "Age_MessageNoticeUpdStatus";

        public override bool IsValid() => UserId > 0;
    }
}
