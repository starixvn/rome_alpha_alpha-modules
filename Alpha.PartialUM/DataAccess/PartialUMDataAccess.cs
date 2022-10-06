namespace Alpha.PartialUM.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Alpha.PartialUM.Models;
    using Fanex.Data;

    public interface IPartialUMDataAccess
    {
        IEnumerable<UMInfo> GetUMInfoList(string market);
    }

    public class PartialUMDataAccess : IPartialUMDataAccess
    {
        public IEnumerable<UMInfo> GetUMInfoList(string market)
        {
            var inputParams = new Hashtable
            {
                ["marketname"] = market
            };

            using (var db = ObjectDbFactory.CreateInstance("Acc_CheckUM_ByReport"))
            {
                using (var reader = db.ExecuteReader(inputParams))
                {
                    while (reader.Read())
                    {
                        yield return ToUMInfo(reader);
                    }
                }
            }
        }

        private static UMInfo ToUMInfo(IDataReader reader)
            => new UMInfo(
                reader["FunctionTypeName"].ToString(),
                Convert.ToDateTime(reader["BeginTime"]),
                Convert.ToDateTime(reader["EndTime"]));
    }
}
