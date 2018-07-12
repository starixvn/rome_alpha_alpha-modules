using System;
using System.Data;
using System.Linq;
using Fanex.Products.Enums;
using Fanex.Products.Extensions;

namespace Fanex.Products.Services
{
    public class Product_33 : Product_23, IProduct
    {
        public Product_33(bool webConfigActive)
            : base(webConfigActive)
        {
        }

        protected override bool? GetSynced()
        {
            return true;
        }

        public override DCSIcon GetDCSIcon(DataRow drow)
        {
            if (ShowSettings)
            {
                var betTypes = new[] { 1401, 1402, 1403, 1404 };
                var isEnabled = betTypes.Any(betType =>
                {
                    string disabledColumnName = "disablepokergames" + betType;
                    object disabled = drow.GetValue(disabledColumnName);

                    return disabled != null && !Convert.ToBoolean(disabled);
                });

                if (isEnabled)
                {
                    return DCSIcon.EnabledEdit;
                }
            }

            return DCSIcon.DisabledEdit;
        }
    }
}
