namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserLevelSpecification : CompositeSpecification<IUser>
    {
        private const string SpecificationId = "e6cacf4d-37d8-4031-a17c-8b02600d4c3f";
        private const string SpecificationName = "User Level Specification";

        private static readonly IDictionary<byte, string> DefaultLevels = new Dictionary<byte, string>
        {
            [0] = "All",
            [2] = "Agent",
            [3] = "Master",
            [4] = "Super"
        };

        public UserLevelSpecification()
        {
            Id = SpecificationId;
            Name = SpecificationName;
        }

        public UserLevelSpecification(IEnumerable<byte> levels)
            : this()
        {
            Levels = levels;
        }

        public override dynamic Value
        {
            get
            {
                return Levels;
            }

            set
            {
                Levels = value is IEnumerable<byte> ? value : Levels;
            }
        }

        public IEnumerable<byte> Levels { get; set; }

        public override dynamic ValueRange
        {
            get
            {
                return DefaultLevels;
            }
        }

        public override bool IsSatisfiedBy(IUser user) =>
             SpecificationOperator == SpecificationOperator.In
                ? IsUserLevelInList(user.LevelId)
                : !IsUserLevelInList(user.LevelId);

        private bool IsUserLevelInList(byte userLevelId) =>
            Levels.Contains((byte)0)
            || Levels.Contains(userLevelId);
    }
}