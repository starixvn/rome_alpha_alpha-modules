namespace Alpha.Announcements.UplineIds.Specs
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Specs;
    using Models;
    using Newtonsoft.Json.Linq;

    public class UplineIdsSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "6b965250-1c50-49f3-865d-a6f8582bda44";
        private const string SpecificationName = "Upline Ids";
        private IEnumerable<int> UplineIds { get; set; }

        public UplineIdsSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public UplineIdsSpecification(IEnumerable<int> uplineIds)
            : this()
        {
            UplineIds = uplineIds;
        }

        public override dynamic Value
        {
            get
            {
                return UplineIds;
            }

            set
            {
                UplineIds = value is IEnumerable ? ((JArray)value).Select(x => (int)x).ToList() : UplineIds;
            }
        }

        public override bool IsSatisfiedBy(IParam param)
         => SpecificationOperator == SpecificationOperator.In
                    ? IsContainsUplineId(param.SRecommendId) || IsContainsUplineId(param.MRecommendId)
                    : !IsContainsUplineId(param.SRecommendId) && !IsContainsUplineId(param.MRecommendId);

        private bool IsContainsUplineId(int uplineId) => UplineIds.Contains(uplineId);
    }
}
