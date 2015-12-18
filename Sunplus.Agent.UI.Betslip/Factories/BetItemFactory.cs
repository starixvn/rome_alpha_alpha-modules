namespace Sunplus.Agent.UI.BetSlip.Factories
{
    using System;
    using System.Data;
    using Sunplus.Agent.UI.BetSlip.UI;
    using Sunplus.Agent.Utilities;

    public class BetItemFactory
    {
        private const string BetListItemNamespace = " Sunplus.Agent.UI.BetSlip.UI.BetType{0}";

        #region BetItem

        /// <summary>
        /// Create IBetItem instance by reflect on bet type id.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        internal static IBetItem CreateItem(DataRow dataRow)
        {
            var betType = Cast.AsInt32(dataRow["bettype"]);
            var typeName = string.Format(BetListItemNamespace, betType);

            var type = Type.GetType(typeName);
            var betItem = ReferenceEquals(type, null)
                    ? new BetType1()
                    : Activator.CreateInstance(type) as IBetItem;

            betItem = betItem ?? new BetType1();

            betItem.Data = dataRow;
            
            return betItem;
        }
        #endregion
    }
}
