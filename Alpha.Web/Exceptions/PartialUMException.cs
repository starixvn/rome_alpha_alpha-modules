namespace Alpha.Web.Exceptions
{
    using System;

    public class PartialUMException : Exception
    {
        public PartialUMException(string umFrom, string umTo)
        {
            UMFrom = umFrom;
            UMTo = umTo;
        }

        public string UMFrom { get; }

        public string UMTo { get; }
    }
}
