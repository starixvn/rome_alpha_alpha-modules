namespace Alpha.CopyAccount.Params
{
    public class CopyAccountParams
    {
        public CopyAccountParams(
            int actorId,
            byte actorUserLevel,
            int actorSubAccId,
            string actorSubAccName,
            string username,
            string password,
            string phone,
            string mobilePhone,
            string fax,
            decimal credit,
            decimal maxCredit,
            int sourceCustId,
            string sourceUsername,
            string language,
            string iP,
            byte systemId,
            bool isQrCode,
            string firstName = null,
            string lastName = null)
        {
            ActorId = actorId;
            ActorUserLevel = actorUserLevel;
            ActorSubAccId = actorSubAccId;
            ActorSubAccName = actorSubAccName;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            MobilePhone = mobilePhone;
            Fax = fax;
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

        public int ActorSubAccId { get; }

        public string ActorSubAccName { get; }

        public string Username { get; }

        public string Password { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Phone { get; }

        public string MobilePhone { get; }

        public string Fax { get; }

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