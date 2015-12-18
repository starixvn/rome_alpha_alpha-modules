namespace Sunplus.Agent.UI.BetSlip.Factories
{
    using System.Data;
    using System.Text;

    public class RenderingFactory
    {
        /// <summary>
        /// Render bet slip 
        /// </summary>
        /// <param name="betSlipData"></param>
        /// <returns></returns>
        public static StringBuilder Render(DataTable betSlipData)
        {
            var content = new StringBuilder();

            var dataRow = betSlipData.Rows[0];
            var betTypeItem = BetItemFactory.CreateItem(dataRow);
            betTypeItem.AssignData();

            betTypeItem.ToString(ref content);

            return content;
        }
    }
}
