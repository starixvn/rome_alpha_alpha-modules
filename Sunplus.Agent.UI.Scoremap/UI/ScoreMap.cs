namespace Sunplus.Agent.UI.ScoreMap.UI
{
    using System;
    using System.Text;

    public class ScoreMap : ScoreMapBase
    {
        #region Properties and define variable
        protected int index = 0, indexMatch = 0;
        protected int indexHome = 0, indexAway = 0;

        StringBuilder content = new StringBuilder();
        StringBuilder main = new StringBuilder();
        StringBuilder sbChild = new StringBuilder();
        StringBuilder sbTotal = new StringBuilder();

        #endregion
        public override void Assign(System.Data.DataRow row)
        {
            decimal total = Convert.ToDecimal(row["total"]);
            this.total = total.ToString("N0");// Formater.DecFormat(total, 0) -> value.ToString("N" + decDigits);
            this.color = total > 0 ? "color:red" : string.Empty;
        }
        #region IComponent Members
        public override string Render()
        {
            return ToString();
        }
        public override void ToString(ref System.Text.StringBuilder bd)
        {
            base.ToString(ref bd);
        }
        public override void HideLinkedResources()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}