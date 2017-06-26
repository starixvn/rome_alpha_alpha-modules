namespace Alpha.Announcements.Specs
{
    using System;

    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public string Id { get; protected set; }

        public string Name { get; protected set; }

        public virtual bool Combinable => true;

        public string TypeName => GetType().AssemblyQualifiedName;

        public ConjunctionOperator ConjunctionOperator { get; set; }

        public SpecificationOperator SpecificationOperator { get; set; }

        public virtual dynamic ValueRange { get; set; }

        public virtual dynamic Value { get; set; }

        public abstract bool IsSatisfiedBy(T o);

        public ISpecification<T> And(ISpecification<T> specification) => new AndSpecification<T>(this, specification);

        public ISpecification<T> Or(ISpecification<T> specification) => new OrSpecification<T>(this, specification);

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        public ISpecification<T> Not(ISpecification<T> specification) => new NotSpecification<T>(specification);

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    }

    public class AndSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> leftSpecification;
        private readonly ISpecification<T> rightSpecification;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.leftSpecification = left;
            this.rightSpecification = right;
        }

        public override bool Combinable => false;

        public override bool IsSatisfiedBy(T o) => this.leftSpecification.IsSatisfiedBy(o)
                && this.rightSpecification.IsSatisfiedBy(o);
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> leftSpecification;
        private readonly ISpecification<T> rightSpecification;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.leftSpecification = left;
            this.rightSpecification = right;
        }

        public override bool Combinable => false;

        public override bool IsSatisfiedBy(T o) => this.leftSpecification.IsSatisfiedBy(o)
                || this.rightSpecification.IsSatisfiedBy(o);
    }

    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> specification;

        public NotSpecification(ISpecification<T> spec)
        {
            this.specification = spec;
        }

        public override bool Combinable => false;

        public override bool IsSatisfiedBy(T o) => !this.specification.IsSatisfiedBy(o);
    }

    public class ExpressionSpecification<T> : CompositeSpecification<T>
    {
        private readonly Func<T, bool> expression;

        public ExpressionSpecification(Func<T, bool> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                this.expression = expression;
            }
        }

        public override bool IsSatisfiedBy(T o) => this.expression(o);
    }
}