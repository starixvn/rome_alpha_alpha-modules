using Fanex.Products.Enums;
using Fanex.Products.Extensions;
using System;
using System.Data;

namespace Fanex.Products.Services
{
    public class Product_5: Product_23, IProduct
    {
        public Product_5(bool webConfigActive)
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
                string lowerName = Name.ToLowerInvariant();

                string disabledColumnName = "disable" + lowerName;
                object disabled = drow.GetValue(disabledColumnName);

                if (disabled != null && !Convert.ToBoolean(disabled))
                {
                    return DCSIcon.EnabledEdit;
                }
            }

            return DCSIcon.DisabledEdit;
        }
    }
}
