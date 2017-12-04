using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fanex.Products.Configurations;
using Fanex.Products.Services;
using Fanex.Products.Utilities;

namespace Fanex.Products
{
    public static class ProductManager
    {
        private static readonly IDictionary<int, BaseProduct> ProductsDictionary = null;

        static ProductManager()
        {
            ProductsDictionary = GetProductsDictionary();
        }

        public static ICollection<BaseProduct> Products
        {
            get
            {
                return ProductsDictionary.Values;
            }
        }

        public static void SetProductsStatusToSession(Hashtable outputParams)
        {
            foreach (BaseProduct product in Products)
            {
                product.SetProductStatusToSession(outputParams);
            }
        }

        public static BaseProduct GetProduct(int productId)
        {
            BaseProduct baseProduct;

            if (ProductsDictionary.TryGetValue(productId, out baseProduct))
            {
                return baseProduct;
            }

            return null;
        }

        public static bool IsActive(int productId)
        {
            BaseProduct baseProduct;

            if (ProductsDictionary.TryGetValue(productId, out baseProduct))
            {
                return baseProduct.Active;
            }

            return false;
        }

        public static bool IsShowReport(int productId)
        {
            BaseProduct baseProduct;

            if (ProductsDictionary.TryGetValue(productId, out baseProduct))
            {
                return baseProduct.ShowReport;
            }

            return false;
        }

        public static bool IsShowSettings(int productId)
        {
            BaseProduct baseProduct;

            if (ProductsDictionary.TryGetValue(productId, out baseProduct))
            {
                return baseProduct.ShowSettings;
            }

            return false;
        }

        public static bool? IsSynced(int productId)
        {
            BaseProduct baseProduct;

            if (ProductsDictionary.TryGetValue(productId, out baseProduct))
            {
                return baseProduct.Synced;
            }

            return new bool?();
        }

        public static bool IsDisabled(int productId)
        {
            BaseProduct baseProduct;

            if (ProductsDictionary.TryGetValue(productId, out baseProduct))
            {
                return baseProduct.Disabled;
            }

            return true;
        }

        public static string GetProductName(int productId)
        {
            return RefDataUtility.GetProductName(productId);
        }

        private static IDictionary<int, BaseProduct> GetProductsDictionary()
        {
            IDictionary<int, BaseProduct> products = new Dictionary<int, BaseProduct>(16);

            BaseProduct product;
            string baseProductNamespace = typeof(BaseProduct).Namespace;

            var productElements = ProductSection.Instance.Products
                .Cast<ProductElement>()
                .Where(p => !string.IsNullOrEmpty(p.StatusHandlerType));

            foreach (ProductElement item in productElements)
            {
                product = (BaseProduct)Activator.CreateInstance(
                    Type.GetType(string.Format("{0}.{1}", baseProductNamespace, item.StatusHandlerType)),
                    BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    new object[] { item.Active },
                    null);

                if (product != null)
                {
                    product.Id = item.Id;
                    product.Name = item.Name;
                    product.SportId = item.SportId;
                    product.ReportSPParam = item.ReportSPParam;
                    product.ViewPTLink = item.ViewPTLink;
                    product.ViewCommLink = item.ViewCommLink;
                    product.MicroSiteNameAppSetting = item.MicroSiteNameAppSetting;

                    product.CustomerSettingLink = item.CustomerSettingLink;
                    product.EnableStatusLink = item.EnableStatusLink;
                    product.SyncCustomerLink = item.SyncCustomerLink;

                    products.Add(product.Id, product);
                }
            }

            return products;
        }
    }
}
