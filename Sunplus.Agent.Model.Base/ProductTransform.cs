namespace Sunplus.Agent.Model.Base
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    public class ProductTransform
    {
        private static readonly Dictionary<int, string> ProductNames = new Dictionary<int, string>
        {
            [19] = "cockfighting",
            [20] = "cricket",
            [21] = "allbet",
            [22] = "voidbridge"
        };

        /// <summary>
        /// convention for product:
        /// hide-productid
        /// disable-productid
        /// synced-productid
        /// </summary>
        private static readonly Dictionary<string, bool> StatusDefaultValues = new Dictionary<string, bool>
        {
            ["hide-"] = true,
            ["disable-"] = true,
            ["synced-"] = false
        };

        public static void TransformProductStatus(UserSessionBase session, Hashtable values)
        {
            TransformProductStatusByName(session, values);
            TransformProductStatusByConvention(session, values);
        }

        public static void TransformProductStatusByName(UserSessionBase session, Hashtable values)
        {
            foreach (var productName in ProductNames)
            {
                session.HideProducts[productName.Key] = GetProductStatus("hide" + productName.Value, values, true);
                session.DisableProducts[productName.Key] = GetProductStatus("disable" + productName.Value, values, true);
                session.SyncProducts[productName.Key] = GetProductStatus("synced" + productName.Value, values, false);
            }
        }

        private static bool GetProductStatus(string productStatus, Hashtable values, bool defaultValue)
        {
            return values.Contains(productStatus) ? (bool)values[productStatus] : defaultValue;
        }

        private static void TransformProductStatusByConvention(UserSessionBase session, Hashtable values)
        {
            var productIds = new List<int>();

            foreach (string key in values.Keys)
            {
                var productId = 0;
                var prefix = StatusDefaultValues
                    .Where(statusPrefix => key.StartsWith(statusPrefix.Key))
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(prefix.Key)
                   || !int.TryParse(key.Substring(prefix.Key.Length), out productId)
                   || productIds.Contains(productId))
                {
                    continue;
                }

                productIds.Add(productId);
            }

            foreach (var productId in productIds)
            {
                var prefix = "hide-";
                session.HideProducts[productId] = GetProductStatus(prefix + productId, values, StatusDefaultValues[prefix]);

                prefix = "disable-";
                session.DisableProducts[productId] = GetProductStatus(prefix + productId, values, StatusDefaultValues[prefix]);

                prefix = "synced-";
                session.SyncProducts[productId] = GetProductStatus(prefix + productId, values, StatusDefaultValues[prefix]);
            }
        }
    }
}
