namespace Alpha.Announcements.Client.Helpers
{
    using System;
    using Models;
    using Newtonsoft.Json.Linq;
    using Specs;

    public class SpecificationConverter : JsonCreationConverter<ISpecification<IParam>>
    {
        protected override Type GetTargetType(Type type, JObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Object cannot be null");
            }

            return Type.GetType(obj.GetValue("TypeName", StringComparison.OrdinalIgnoreCase).ToString());
        }
    }
}