namespace Fanex.BetListDetail.ColossusBets.Models
{
    public class MemberChoiceResultModel
    {
        public MemberChoiceResultModel(string iframe, TokenResultModel tokenResult)
        {
            Iframe = iframe;
            TokenResult = tokenResult;
        }

        public string Iframe { get; }

        public TokenResultModel TokenResult { get; }
    }
}
