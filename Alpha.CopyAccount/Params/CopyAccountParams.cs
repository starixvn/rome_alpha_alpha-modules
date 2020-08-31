namespace Alpha.CopyAccount.Params
{
    public class CopyAccountParams
    {
        public CopyAccountParams(int actorId, byte actorUserLevel, string username, string password, string phone, string mobilePhone, string fax, decimal credit, decimal maxCredit, int sourceCustId, string sourceUsername, string language, string iP, byte systemId, bool isQrCode)
        {
            ActorId = actorId;
            ActorUserLevel = actorUserLevel;
            Username = username;
            Password = password;
            Credit = credit;
            MaxCredit = maxCredit;
            SourceCustId = sourceCustId;
            SourceUsername = sourceUsername;
            Language = language;
            IP = iP;
            SystemId = systemId;
            IsQrCode = isQrCode;
            Phone = phone;
            MobilePhone = mobilePhone;
            Fax = fax;
        }

        public string Phone { get; }

        public string MobilePhone { get; }

        public string Fax { get; }

        public string Password { get; }

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