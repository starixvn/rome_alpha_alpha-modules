namespace Alpha.Announcements.UnsynchronizedSetting.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public interface UnsynchronizedSettingDataAccess
    {
        Dictionary<string, int> GetUnconfirmSettingFilterDict(ICriteria criteria);
    }

    public class UnsynchronizedSettingDataAccessImpl : UnsynchronizedSettingDataAccess
    {
        public Dictionary<string, int> GetUnconfirmSettingFilterDict(ICriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            Dictionary<string, int> data = new Dictionary<string, int>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            data.Add(reader.ToString("ProductName"), reader.ToInt("ProductId"));
                        }
                    }
                }
            }

            return data;
        }
    }
}
