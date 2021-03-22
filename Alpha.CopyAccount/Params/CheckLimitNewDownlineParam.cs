namespace Alpha.CopyAccount.Params
{
    public class CheckLimitNewDownlineParam
    {
        public CheckLimitNewDownlineParam(int custId, byte custLevel, string language)
        {
            CustId = custId;
            CustLevel = custLevel;
            Language = language;
        }

        public int CustId { get; }

        public byte CustLevel { get; }

        public string Language { get; }
    }
}
