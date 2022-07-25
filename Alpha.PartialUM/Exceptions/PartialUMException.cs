namespace Alpha.PartialUM.Exceptions
{
    using System;

    public sealed class PartialUMException : Exception
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
