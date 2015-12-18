namespace Sunplus.Agent.UI.BetSlip.UI
{
    using System;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// 3-way handicap
    /// </summary>
    public class BetType28 : BetType1
    {
        public override void AssignChoice()
        {
            var betTeam = Data["betteam"].ToString();

            switch (betTeam)
            {
                case "1":
                    Choice = CacheRef.GetTeamName(Data["homeid"]);
                    break;
                case "2":
                    Choice = CacheRef.GetTeamName(Data["awayid"]);
                    break;
                default:
                    Choice = LocalResources.BetSlipUILang.lblDraw;
                    break;
            }
        }

        public override void AssignHdp()
        {
            var betTeam = Data["betteam"].ToString();
            var hdp1 = Cast.AsDecimal(Data["hdp1"]);
            var hdp2 = Cast.AsDecimal(Data["hdp2"]);

            string handicap;

            if ("1" == betTeam)
            {
                handicap = RedForNegativeAmount((int)(hdp2 - hdp1), 0);
            }
            else if ("2" == betTeam)
            {
                handicap = RedForNegativeAmount((int)(hdp1 - hdp2), 0);
            }
            else
            {
                Choice = LocalResources.BetSlipUILang.lblDraw;
                if (hdp1 - hdp2 > 0) handicap = CacheRef.GetTeamName(Data["awayid"]) + " +";
                else if (hdp1 - hdp2 < 0) handicap = CacheRef.GetTeamName(Data["homeid"]) + " +";
                else handicap = "";
                handicap += RedForNegativeAmount((int)Math.Abs(hdp2 - hdp1), 0);
            }
            Hdp = handicap;
        }

        private string RedForNegativeAmount(object Amount, byte decDigits)
        {
            string str = "";
            if (Cast.AsDecimal(Formater.DecFormat(Amount, decDigits)) < 0)
                str = "<font color='#eb6003'>" + Formater.DecFormat(Amount, decDigits) + "</font>";
            else
                str = Formater.DecFormat(Amount, decDigits);
            return str;
        }
        
    }
}