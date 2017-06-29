namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserIdSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "66F31C82-6EB1-4296-98E6-7DC593E80B1D";
        private const string SpecificationName = "User Ids";

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
                UserIds = value is IEnumerable<int> ? value : UserIds;
            }
        }

        public IEnumerable<int> UserIds { get; set; }

        public override bool IsSatisfiedBy(IParam user) =>
            SpecificationOperator == SpecificationOperator.In
                    ? IsContainsUserId(user.Id)
                    : !IsContainsUserId(user.Id);

        private bool IsContainsUserId(int userId) => UserIds.Contains(userId);
    }
}