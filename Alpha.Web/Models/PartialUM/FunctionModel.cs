namespace Alpha.Web.Models.PartialUM
{
    public class FunctionModel
    {
        public FunctionModel(string name, string umFrom, string umTo)
        {
            Name = name;
            UMFrom = umFrom;
            UMTo = umTo;
        }

        public string Name { get; }

        public string UMFrom { get; }

        public string UMTo { get; }
    }
}