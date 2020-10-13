namespace Alpha.CopyAccount.Models
{
    public class CopyAccountResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccess => Code == 0;

        public int CustId { get; set; }

        public string CustName { get; set; }

        public string Password { get; set; }

        public int CustLevel { get; set; }

        public string SourceUsername { get; set; }

        public decimal Credit { get; set; }

        public decimal MaxCredit { get; set; }
    }
}