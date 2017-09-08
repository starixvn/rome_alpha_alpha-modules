namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using System.Collections;

    public class UserIdSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "66f31c82-6eb1-4296-98e6-7dc593e80b1d";
        private const string SpecificationName = "User Ids";
        private IEnumerable<int> UserIds { get; set; }

        public UserIdSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public UserIdSpecification(IEnumerable<int> userIds)
            : this()
        {
            UserIds = userIds;
        }

        public override dynamic Value
        {
            get
            {
                return UserIds;
            }

            set
            {
                UserIds = value is IEnumerable ? ((JArray)value).Select(x => (int)x).ToList() : UserIds;
            }
        }

        public override bool IsSatisfiedBy(IParam param) =>
            SpecificationOperator == SpecificationOperator.In
                    ? IsContainsUserId(param.UserId)
                    : !IsContainsUserId(param.UserId);

        private bool IsContainsUserId(int userId) => UserIds.Contains(userId);
    }
}