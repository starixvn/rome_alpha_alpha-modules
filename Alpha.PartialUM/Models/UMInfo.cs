namespace Alpha.PartialUM.Models
{
    using System;

    public sealed class UMInfo
    {
        public UMInfo()
        {
        }

        public UMInfo(
            string functionTypeName,
            DateTime beginTime,
            DateTime endTime)
        {
            FunctionTypeName = functionTypeName;
            BeginTime = beginTime;
            EndTime = endTime;
        }

        public string FunctionTypeName { get; }

        public DateTime BeginTime { get; }

        public DateTime EndTime { get; }
    }
}
