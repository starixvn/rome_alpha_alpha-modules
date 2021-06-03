namespace Alpha.Web.Models.PartialUM
{
    public sealed class FunctionModel
    {
        public FunctionModel(string umFrom, string umTo)
        {
            UMFrom = umFrom;
            UMTo = umTo;
        }

        public string UMFrom { get; }

        public string UMTo { get; }
    }
}