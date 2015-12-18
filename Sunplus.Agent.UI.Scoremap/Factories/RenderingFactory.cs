namespace Sunplus.Agent.UI.ScoreMap.Factories
{
    using System;
    using System.Data;
    using System.Text;
    using Sunplus.Agent.Model.BL.Cache;
    using UI;

    public class RenderingFactory
    {
        private static int NUM_COLUMN = 7;
        private static int ALL_ITEM = 49;
        public static void Render(Boolean isSmartPhone, DataTable listScoreMap, DataTable listMatch , StringBuilder content, bool isRefresh)
        {
            // Append content
            DataRowCollection rowsScoreMap = listScoreMap.Rows;
            DataRowCollection rowsMatch = listMatch.Rows;
            int count = rowsScoreMap.Count;
            // Append content
            ContentRender(isSmartPhone, content, rowsScoreMap, rowsMatch, count, isRefresh);
        }

        private static void ContentRender(Boolean isSmartPhone, StringBuilder content, DataRowCollection rowsScoreMap, DataRowCollection rowsMatch, int count, bool isRefresh)
        {
            int item = 0, numRows = 0, posCurrent = 0;
            string matchId = "0";

            StringBuilder  sbRows = new StringBuilder();
            StringBuilder sbColumn = new StringBuilder();
            //List Match
            foreach (DataRow rowMatch in rowsMatch)
            {
                matchId = rowMatch["matchid"].ToString();
                int column = 0; 
                // Render row collection
                for (int i = posCurrent; i < count; i++)
                {
                    if (item < ALL_ITEM)
                    {
                        if (column < NUM_COLUMN)
                        {
                            DataRow row = rowsScoreMap[i];
                            if (row["matchid"].ToString() != matchId) break;
                            IScoreMapItem scoreMapItem = new ScoreMap();
                            scoreMapItem.Assign(row);
                            scoreMapItem.ToString(ref sbColumn);
                            posCurrent++;
                        }
                        item++;
                        column++;
                    }

                    if (column == NUM_COLUMN)
                    {
                        RowsRender(sbRows, sbColumn, numRows);
                        sbColumn = new StringBuilder();
                        column = 0;
                        numRows++;
                    }
                }
                //Append Header
                HeaderRender(isSmartPhone, content, sbRows, rowMatch, isRefresh);
                sbRows = new StringBuilder();
                //Append Footer
                FooterRender(content, isRefresh);
                item = numRows = 0;
            }
        }

        private static void HeaderRender(Boolean isSmartPhone, StringBuilder content, StringBuilder sbRows, DataRow row, bool isRefresh)
        {
            HeaderScoreMap_Template header = new HeaderScoreMap_Template();
            string matchId = row["matchid"].ToString();
            if (isSmartPhone)
            {
                header.SmartPhoneMatchName.Home = CacheRef.GetTeamName(row["homeid"]);
                header.SmartPhoneMatchName.Away = CacheRef.GetTeamName(row["awayid"]);
                header.SmartPhoneMatchName.matchId = matchId;
                header.FullSiteMatchName = null;
            }
            else
            {
                header.FullSiteMatchName.Home = CacheRef.GetTeamName(row["homeid"]);
                header.FullSiteMatchName.Away = CacheRef.GetTeamName(row["awayid"]);
                header.FullSiteMatchName.matchId = matchId;
                header.SmartPhoneMatchName = null;
            }
            
            header.matchId = matchId;
            header.Content = sbRows.ToString();
            if (isRefresh)
            {
                header.RefeshContent = null;
            }
            else
            {
                header.RefeshContent.matchId = matchId;
            }
            header.ToString(ref content);
        }

        private static void RowsRender(StringBuilder sbRows, StringBuilder column, int item)
        {
            RowScoreMap_Template content = new RowScoreMap_Template();
            content.Num = item.ToString();
            content.ChildTotal.SetValue(column.ToString());
            content.ToString(ref sbRows);
        }

        private static void FooterRender(StringBuilder content, bool isRefresh)
        {
            FooterScoreMap_Template footer = new FooterScoreMap_Template();
            if (isRefresh)
            {
                footer.RefeshContent = null;
            }
            footer.ToString(ref content);
        }
    }
}