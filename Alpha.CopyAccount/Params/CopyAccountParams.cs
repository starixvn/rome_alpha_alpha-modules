namespace Alpha.CopyAccount.Params
{
    public class CopyAccountParams
    {
        public CopyAccountParams(int actorId, byte actorUserLevel, string username, decimal credit, decimal maxCredit, int sourceCustId, string sourceUsername, string language, string iP, byte systemId, bool isQrCode)
        {
            ActorId = actorId;
            ActorUserLevel = actorUserLevel;
            Username = username;
            Credit = credit;
            MaxCredit = maxCredit;
            SourceCustId = sourceCustId;
            SourceUsername = sourceUsername;
            Language = language;
            IP = iP;
            SystemId = systemId;
            IsQrCode = isQrCode;
        }

        public int ActorId { get; }

        public byte ActorUserLevel { get; }

        public string Username { get; }

        public decimal Credit { get; }

        public decimal MaxCredit { get; }

        public int SourceCustId { get; }

        public string SourceUsername { get; }

        public string Language { get; }

        public string IP { get; }

        public byte SystemId { get; }

        public bool IsQrCode { get; }
    }
}