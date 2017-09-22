namespace Alpha.Announcements.UnsynchronizedSetting.Specs
{
    using System.Collections;
    using System.Collections.Generic;
    using Announcements.Specs;
    using Models;

    public class UnsynchronizedSettingSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "6958e609-fd9c-4758-b757-e74e0466afd4";
        private const string SpecificationName = "Unsynchronized Setting";
        public Dictionary<string, int> Products { get; set; }

        public UnsynchronizedSettingSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public UnsynchronizedSettingSpecification(Dictionary<string, int> products)
            : this()
        {
            Products = products;
        }

        public UnsynchronizedSettingSpecification(int userId)
           : this()
        {
            Products = GetProducts(userId);
        }

        //public override dynamic Value
        //{
        //    get
        //    {
        //        return Products;
        //    }

        //    set
        //    {
        //        Products = value is IEnumerable ? ((JArray)value).Select(x => (int)x).ToList() : Products;
        //    }
        //}

        public override bool IsSatisfiedBy(IParam param)
        {
            var userId = GetUserId(param);
            Products = GetProducts(userId);

            return Products.Count > 0;
        }

        private int GetUserId(IParam param)
        {
            if (param.IsSuper) return param.SRecommendId;
            if (param.IsMaster) return param.MRecommendId;
            return param.ARecommendId;
        }

        private Dictionary<string, int> GetProducts(int userId)
        {
            var products = new Dictionary<string, int> ()
            {
                { "Casino", 1},
                { "Gold Deluxe", 2}
            };

            return products;
        }
    }
}
