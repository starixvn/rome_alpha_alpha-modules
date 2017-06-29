namespace Alpha.Announcements.Specs
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserLevelSpecification : CompositeSpecification<IParam>
    {
        public const string SpecificationId = "e6cacf4d-37d8-4031-a17c-8b02600d4c3f";
        private const string SpecificationName = "User Level";

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

        public override bool IsSatisfiedBy(IParam user) =>
             SpecificationOperator == SpecificationOperator.In
                ? IsUserLevelInList(user.LevelId)
                : !IsUserLevelInList(user.LevelId);

        private bool IsUserLevelInList(byte userLevelId) =>
            Levels.Contains((byte)0)
            || Levels.Contains(userLevelId);
    }
}