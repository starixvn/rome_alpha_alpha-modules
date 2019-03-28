namespace Sunplus.Agent.Model.Base
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Entities;

    public class ProductTransform
    {
        private static readonly Dictionary<int, string> ProductNames = LoadProductNameMappings();

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
            ["synced-"] = true
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
                session.SetHideProduct(productName.Key, GetProductStatus("hide" + productName.Value, values, true));
                session.SetDisabledProduct(productName.Key, GetProductStatus("disable" + productName.Value, values, true));
                session.SetSyncedProduct(productName.Key, GetProductStatus("synced" + productName.Value, values, true));
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
                session.SetHideProduct(productId, GetProductStatus(prefix + productId, values, StatusDefaultValues[prefix]));

                prefix = "disable-";
                session.SetDisabledProduct(productId, GetProductStatus(prefix + productId, values, StatusDefaultValues[prefix]));

                prefix = "synced-";
                session.SetSyncedProduct(productId, GetProductStatus(prefix + productId, values, StatusDefaultValues[prefix]));
            }
        }

        private static Dictionary<int, string> LoadProductNameMappings()
        {
            var result = new Dictionary<int, string>();
            var productText = ConfigurationManager.AppSettings["ProductColumnNameMappings"];
            if (!string.IsNullOrEmpty(productText))
            {
                var products = productText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var product in products)
                {
                    var items = product.Split('=');
                    if (items.Length == 2)
                    {
                        var id = int.Parse(items[0]);
                        var name = items[1].ToLowerInvariant();

                        result.Add(id, name);
                    }
                }
            }

            return result;
        }
    }
}