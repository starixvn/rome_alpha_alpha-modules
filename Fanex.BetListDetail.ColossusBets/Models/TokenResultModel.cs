namespace Fanex.BetListDetail.ColossusBets.Models
{
    public class TokenResultModel
    {
        public TokenResultModel()
        {
            Data = new TokenDetailModel();
        }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public TokenDetailModel Data { get; set; }
    }
}
