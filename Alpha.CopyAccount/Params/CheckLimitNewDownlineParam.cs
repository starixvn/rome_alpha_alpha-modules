namespace Alpha.CopyAccount.Params
{
    public class CheckLimitNewDownlineParam
    {
        public CheckLimitNewDownlineParam(int custId, byte custLevel, string language, string actorUsername)
        {
            CustId = custId;
            CustLevel = custLevel;
            Language = language;
            ActorUsername = actorUsername;
        }

        public int CustId { get; }

        public byte CustLevel { get; }

        public string Language { get; }

        public string ActorUsername { get; }
    }
}
