namespace Fanex.Age.UI
{
    using System.Collections.Generic;
    using Constants;
    using CustomerSetting.Module.KenoStatus.Configurations;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities.Permission;

    public class ProductRenderUtility
    {
        public static Dictionary<int, bool> PermissionDictionary { get; set; }

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
            PermissionDictionary = new Dictionary<int, bool>();
            PermissionDictionary.Add(ProductIdConstants.Sportsbook, true);
            PermissionDictionary.Add(ProductIdConstants.Racing, UserPermission.IsShowRacing);
            PermissionDictionary.Add(ProductIdConstants.RNGCasino, UserPermission.IsShowCasino);
            PermissionDictionary.Add(ProductIdConstants.Bingo, UserPermission.IsShowBingo && !AppSettings.DisabledBingoCommission);
            PermissionDictionary.Add(ProductIdConstants.LiveCasino, UserPermission.IsShowLiveCasino);
            PermissionDictionary.Add(ProductIdConstants.GoldDeluxe, UserPermission.IsShowGoldDeluxeSettingForCredit);
            PermissionDictionary.Add(ProductIdConstants.NumberGame, UserPermission.IsShowNumberGame);
            PermissionDictionary.Add(ProductIdConstants.VirtualSports, UserPermission.IsShowVirtualSports);
            PermissionDictionary.Add(ProductIdConstants.Keno, UserPermission.IsShowKenoSetting && KenoSection.Instance.ShowSetting);
            PermissionDictionary.Add(ProductIdConstants.Sportsbook2, UserPermission.IsShowSportsbook2);
            PermissionDictionary.Add(ProductIdConstants.CockFighting, UserPermission.IsEnableProduct(ProductIdConstants.CockFighting));
            PermissionDictionary.Add(ProductIdConstants.ColossusBets, UserPermission.IsShowColossusBets);
            PermissionDictionary.Add(ProductIdConstants.P2P, AppSettings.IsEnableP2P);
            PermissionDictionary.Add(ProductIdConstants.ProgressiveBonus, UserPermission.IsShowCasino);

            return PermissionDictionary;
        }
    }
}