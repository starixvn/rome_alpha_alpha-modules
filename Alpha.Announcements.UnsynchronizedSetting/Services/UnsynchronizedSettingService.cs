namespace Alpha.Announcements.UnsynchronizedSetting.Services
{
    using System.Collections.Generic;
    using DataAccesses;
    using Fanex.Data.Repository;

    public interface UnsynchronizedSettingService
    {
        Dictionary<string, int> GetUnconfirmSettingFilterDict(ICriteria criteria);
    }

    public class UnsynchronizedSettingServiceImpl : UnsynchronizedSettingService
    {
        private readonly UnsynchronizedSettingDataAccess unsynchronizedSettingDataAccess;

        public UnsynchronizedSettingServiceImpl() : this(null)
        {
        }

        public UnsynchronizedSettingServiceImpl(UnsynchronizedSettingDataAccess unsynchronizedSettingDataAccess)
        {
            this.unsynchronizedSettingDataAccess = unsynchronizedSettingDataAccess;
        }

        public Dictionary<string, int> GetUnconfirmSettingFilterDict(ICriteria criteria)
            => unsynchronizedSettingDataAccess.GetUnconfirmSettingFilterDict(criteria);
    }
}
