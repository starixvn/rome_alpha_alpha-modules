namespace Fanex.Age.UI
{
    using CustomerSetting.Module.KenoStatus.Configurations;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Definitions;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Collections.Generic;

    public class ProductRenderUtility
    {
        public static List<int> GetProductHasReport()
        {
            var result = new List<int>();
            result.Add(ProductIdConstants.Sportsbook);
            result.Add(ProductIdConstants.Racing);
            result.Add(ProductIdConstants.RNGCasino);
            result.Add(ProductIdConstants.NumberGame);
            result.Add(ProductIdConstants.Bingo);
            result.Add(ProductIdConstants.P2P);
            result.Add(ProductIdConstants.LiveCasino);
            result.Add(ProductIdConstants.VirtualSports);
            result.Add(ProductIdConstants.Keno);
            result.Add(ProductIdConstants.GoldDeluxe);
            result.Add(ProductIdConstants.ColossusBets);
            result.Add(ProductIdConstants.CockFighting);
            return result;
        }

        public static Dictionary<int, bool> GetPermissionDictionary()
        {
            var result = new Dictionary<int, bool>();
            result.Add(ProductIdConstants.Sportsbook, true);
            result.Add(ProductIdConstants.Racing, UserPermission.IsShowRacing);
            result.Add(ProductIdConstants.RNGCasino, UserPermission.IsShowCasino);
            result.Add(ProductIdConstants.Bingo, UserPermission.IsShowBingo && !AppSettings.DisabledBingoCommission);
            result.Add(ProductIdConstants.LiveCasino, UserPermission.IsShowLiveCasino);
            result.Add(ProductIdConstants.GoldDeluxe, UserPermission.IsShowGoldDeluxeSettingForCredit);
            result.Add(ProductIdConstants.NumberGame, UserPermission.IsShowNumberGame);
            result.Add(ProductIdConstants.VirtualSports, UserPermission.IsShowVirtualSports);
            result.Add(ProductIdConstants.Keno, UserPermission.IsShowKenoSetting && KenoSection.Instance.ShowSetting);
            result.Add(ProductIdConstants.Sportsbook2, UserPermission.IsShowSportsbook2);
            result.Add(ProductIdConstants.CockFighting, UserPermission.IsEnableProduct(ProductIdConstants.CockFighting));
            result.Add(ProductIdConstants.ColossusBets, UserPermission.IsShowColossusBets);
            result.Add(ProductIdConstants.P2P, AppSettings.IsEnableP2P);
            result.Add(ProductIdConstants.ProgressiveBonus, UserPermission.IsShowCasino);

            return result;
        }
    }
}