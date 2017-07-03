namespace Sunplus.Agent.Model.DL.BetList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data.DNAide;

    [Obsolete("Should rename to Sunplus.Agent.Model.BetList")]
    public class BetListDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        #region BalanceUpDay

        public DataTable Age_SelBalanceUpDay()
        {
            return this.ExecSpTable("Age_SelBalanceUpDay", new Hashtable());
        }

        #endregion BalanceUpDay

        #region GetWinLossByDateAndMem

        /// <summary>
        /// Report/WinLoss --> BetList
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <param name="filterParams"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinLossByDate_Mem(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable
            {
                { "custid", custId },
                { "fromdate", dateFrom },
                { "todate", dateTo },
                { "language", language },
                { "movebetsday", moveBetsDate }
            };

            foreach (var element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            DataSet result = ExecSpDataset("Age_BetList_SelWinLossByDate_Mem", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                var error = new AgentDataAccessException("Age_BetList_SelWinLossByDate_Mem", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        /// <summary>
        /// Report/WinLoss --> BetList
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="filterParams"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinLossByDate_Mem_Old(int custId, DateTime dateFrom, DateTime dateTo, string language, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable
            {
                { "custid", custId },
                { "fromdate", dateFrom },
                { "todate", dateTo },
                { "language", language }
            };

            foreach (var element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            DataSet result = ExecSpDataset("Age_BetList_SelWinLossByDate_Mem_Old", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }

            // Throw DataException and log the SP calling
            var error = new AgentDataAccessException("Age_BetList_SelWinLossByDate_Mem_Old", parameters);
            Sunplus.Agent.Utilities.Logs.Write(error);
            throw error;
        }

        #endregion GetWinLossByDateAndMem

        #region WinlossOutright

        /// <summary>
        /// BetList -> Get winloss outright
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossOutright_Ag(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, int leagueId)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "fromdate", dateFrom },
                { "todate", dateTo },
                { "language", language },
                { "movebetsday", moveBetsDate },
                { "leagueid", leagueId }
            };

            return ExecSpTable("Age_BetList_SelWinlossOutright_Ag", parameters);
        }

        /// <summary>
        /// BetList -> Get winloss outright
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossOutright_MAg(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, int leagueId)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "fromdate", dateFrom },
                { "todate", dateTo },
                { "language", language },
                { "movebetsday", moveBetsDate },
                { "leagueid", leagueId }
            };

            return ExecSpTable("Age_BetList_SelWinlossOutright_MAg", parameters);
        }

        /// <summary>
        /// BetList -> Get winloss outright
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossOutright_SAg(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, int leagueId)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "fromdate", dateFrom },
                { "todate", dateTo },
                { "language", language },
                { "movebetsday", moveBetsDate },
                { "leagueid", leagueId }
            };

            return ExecSpTable("Age_BetList_SelWinlossOutright_SAg", parameters);
        }

        #endregion WinlossOutright

        #region WinlossHR

        /// <summary>
        /// Get agent winloss in horse racing
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="leagueId"></param>
        /// <param name="race"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossHR_Ag(int custId, int betType, int leagueId, string race, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, out DataTable outDataTable)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "bettype", betType },
                { "leagueid", leagueId },
                { "race", race },
                { "fromdate", dateFrom },
                { "todate", dateTo },
                { "language", language },
                { "movebetsday", moveBetsDate }
            };

            // return ExecSpTable("Age_BetList_SelWinlossHR_Ag", parameters);
            var result = ExecSpDataset("Age_BetList_SelWinlossHR_Ag", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }

            // Throw DataException and log the SP calling
            var error = new AgentDataAccessException("Age_BetList_SelWinlossHR_Ag", parameters);
            Sunplus.Agent.Utilities.Logs.Write(error);
            throw error;
        }

        /// <summary>
        /// Get master winloss in horse racing
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="leagueId"></param>
        /// <param name="race"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossHR_MAg(int custId, int betType, int leagueId, string race, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, out DataTable outDataTable)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "bettype", betType },
                { "leagueid", leagueId },
                { "race", race },
                { "fromdate", dateFrom },
                { "todate", dateTo },
                { "language", language },
                { "movebetsday", moveBetsDate }
            };

            //// return ExecSpTable("Age_BetList_SelWinlossHR_MAg", parameters);
            var result = ExecSpDataset("Age_BetList_SelWinlossHR_MAg", parameters);

            //// Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            //// Throw DataException and log the SP calling
            var error = new AgentDataAccessException("Age_BetList_SelWinlossHR_MAg", parameters);
            Sunplus.Agent.Utilities.Logs.Write(error);
            throw error;
        }

        /// <summary>
        /// Get super winloss in horse racing
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="leagueId"></param>
        /// <param name="race"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossHR_SAg(int custId, int betType, int leagueId, string race, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, out DataTable outDataTable)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "bettype", betType },
                { "leagueid", leagueId },
                { "race", race },
                { "fromdate", dateFrom },
                { "todate", dateTo },
                { "language", language },
                { "movebetsday", moveBetsDate }
            };

            // return ExecSpTable("Age_BetList_SelWinlossHR_SAg", parameters);
            DataSet result = ExecSpDataset("Age_BetList_SelWinlossHR_SAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }

            // Throw DataException and log the SP calling
            var error = new AgentDataAccessException("Age_BetList_SelWinlossHR_SAg", parameters);
            Sunplus.Agent.Utilities.Logs.Write(error);
            throw error;
        }

        #endregion WinlossHR

        #region WinlossByBetTypeMatch

        /// <summary>
        /// Get agent Winloss by match
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossByBettypeMatch_Ag(int custId, int matchId, int betType, string language, string userName, int liveindicator, out DataTable outDataTable)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "matchid", matchId },
                { "bettype", betType },
                { "language", language },
                { "username", userName },
                { "liveindicator", liveindicator }
            };

            DataSet result = ExecSpDataset("Age_BetList_SelWinlossByBettypeMatch_Ag", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelWinlossByBettypeMatch_Ag", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        /// <summary>
        /// Get master Winloss by match
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossByBettypeMatch_MAg(int custId, int matchId, int betType, string language, string userName, int liveindicator, out DataTable outDataTable)
        {
            var parameters = new Hashtable
            {
                { "custid", custId },
                { "matchid", matchId },
                { "bettype", betType },
                { "language", language },
                { "username", userName },
                { "liveindicator", liveindicator }
            };

            DataSet result = ExecSpDataset("Age_BetList_SelWinlossByBettypeMatch_MAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelWinlossByBettypeMatch_MAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        /// <summary>
        /// Get super Winloss by match
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossByBettypeMatch_SAg(int custId, int matchId, int betType, string language, string userName, int liveindicator, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("username", userName);
            parameters.Add("liveindicator", liveindicator);

            DataSet result = ExecSpDataset("Age_BetList_SelWinlossByBettypeMatch_SAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelWinlossByBettypeMatch_SAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        #endregion WinlossByBetTypeMatch

        #region RunningByDateAndMem

        public DataTable Age_BetList_SelRunningByDate_Mem(int custId, string userName, DateTime dateFrom, DateTime dateTo, string language, int getAll, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable dataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("getall", getAll);
            parameters.Add("language", language);
            parameters.Add("username", userName);

            foreach (KeyValuePair<string, bool> element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            DataSet result = ExecSpDataset("Age_BetList_SelRunningByDate_Mem", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                dataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelRunningByDate_Mem", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        #endregion RunningByDateAndMem

        #region AllBetsByDateAndMem

        public DataTable Age_BetList_SelAllByDate_Mem(int custId, int agentId, string userName, DateTime dateFrom, DateTime dateTo, string language, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("agid", agentId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("username", userName);

            foreach (KeyValuePair<string, bool> element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            DataSet result = ExecSpDataset("Age_BetList_SelAllByDate_Mem", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelAllByDate_Mem", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        #endregion AllBetsByDateAndMem

        #region DeletedBets

        /// <summary>
        /// Get deleted bets
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelDeleted(int custId, int roleId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("roleid", roleId);
            parameters.Add("language", language);
            return ExecSpTable("Age_BetList_SelDeleted", parameters);
        }

        /// <summary>
        /// Get deleted bets
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="language"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelDeleted(int custId, int roleId, string language, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("roleid", roleId);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelDeleted", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelDeleted", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        #endregion DeletedBets

        #region AllHRByDate

        /// <summary>
        /// Get horse racing bet by matchid for agent
        /// @HanhVu: Select data for Betlist of HR (link from Total Bets==>HR)
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="matchId"></param>
        /// <param name="language"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelAllByDateHR_Ag(int custId, int betType, int matchId, string language, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("matchid", matchId);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelAllByDateHR_Ag", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelAllByDateHR_Ag", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        /// <summary>
        /// Get horse racing bet by matchid for master
        /// @HanhVu: Select data for Betlist of HR (link from Total Bets==>HR)
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="matchId"></param>
        /// <param name="language"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelAllByDateHR_MAg(int custId, int betType, int matchId, string language, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("matchid", matchId);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelAllByDateHR_MAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelAllByDateHR_MAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        /// <summary>
        /// Get horse racing bet by matchid for super
        /// @HanhVu: Select data for Betlist of HR (link from Total Bets==>HR)
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="matchId"></param>
        /// <param name="language"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelAllByDateHR_SAg(int custId, int betType, int matchId, string language, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("matchid", matchId);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelAllByDateHR_SAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelAllByDateHR_SAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        #endregion AllHRByDate

        #region RunningByBettypeAndMatch

        /// <summary>
        /// Get Running bets by bettype and match for agent
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_Ag(int matchId, int custId, int betType, string language, int liveindicator, string betteam)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            return ExecSpTable("Age_BetList_SelRunningByBettypeMatch_Ag", parameters);
        }

        /// <summary>
        /// Get Running bets by bettype and match for agent
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="liveindicator"></param>
        /// <param name="betteam"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_Ag(int matchId, int custId, int betType, string language, int liveindicator, string betteam, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            DataSet result = ExecSpDataset("Age_BetList_SelRunningByBettypeMatch_Ag", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelRunningByBettypeMatch_Ag", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        /// <summary>
        /// Get Running bets by bettype and match for master
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_MAg(int matchId, int custId, int betType, string language, int liveindicator, string betteam)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            return ExecSpTable("Age_BetList_SelRunningByBettypeMatch_MAg", parameters);
        }

        /// <summary>
        /// Get Running bets by bettype and match for master
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="liveindicator"></param>
        /// <param name="betteam"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_MAg(int matchId, int custId, int betType, string language, int liveindicator, string betteam, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            DataSet result = ExecSpDataset("Age_BetList_SelRunningByBettypeMatch_MAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelRunningByBettypeMatch_MAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        /// <summary>
        /// Get Running bets by bettype and match for super
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_SAg(int matchId, int custId, int betType, string language, int liveindicator, string betteam)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);
            return ExecSpTable("Age_BetList_SelRunningByBettypeMatch_SAg", parameters);
        }

        /// <summary>
        /// Get Running bets by bettype and match for super
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="liveindicator"></param>
        /// <param name="betteam"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_SAg(int matchId, int custId, int betType, string language, int liveindicator, string betteam, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            DataSet result = ExecSpDataset("Age_BetList_SelRunningByBettypeMatch_SAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelRunningByBettypeMatch_SAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        #endregion RunningByBettypeAndMatch

        #region DailyStatement

        /// <summary>
        /// Get dailly statement
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="transType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelByDayAll_Mem(int custId, DateTime dateFrom, int transType, string language, DateTime moveBetsDay, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("transtype", transType);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDay);

            DataSet result = ExecSpDataset("Age_BetList_SelByDayAll_Mem", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelByDayAll_Mem", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        /// <summary>
        /// Get dailly statement old data from table 14
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelByDayAll_Mem_Old(int custId, DateTime dateFrom, string language, int transType, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("language", language);
            parameters.Add("transtype", transType);

            DataSet result = ExecSpDataset("Age_BetList_SelByDayAll_Mem_Old", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                AgentDataAccessException error = new AgentDataAccessException("Age_BetList_SelByDayAll_Mem_Old", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error);
                throw error;
            }
        }

        #endregion DailyStatement

        #region RunningByBettype

        /// <summary>
        /// Get running bets by bet type for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettype_Ag(int custId, int betType, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);

            return ExecSpTable("Age_BetList_SelRunningByBettype_Ag", parameters);
        }

        /// <summary>
        /// Get running bets by bet type for master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettype_MAg(int custId, int betType, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);

            return ExecSpTable("Age_BetList_SelRunningByBettype_MAg", parameters);
        }

        /// <summary>
        /// Get running bets by bet type for super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettype_SAg(int custId, int betType, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);

            return ExecSpTable("Age_BetList_SelRunningByBettype_SAg", parameters);
        }

        /// <summary>
        /// Get correct score for Agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel430_Ag(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_Sel430_Ag", parameters);
        }

        /// <summary>
        /// Get correct score for Master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel430_MAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_Sel430_MAg", parameters);
        }

        /// <summary>
        /// Get correct score for Super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel430_SAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_Sel430_SAg", parameters);
        }

        #endregion RunningByBettype

        #region ParlayWinloss

        /// <summary>
        /// Details for MixParlay for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossParlay_Ag(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, int bettype)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);
            parameters.Add("bettype", bettype);
            return ExecSpTable("Age_BetList_SelWinlossParlay_Ag", parameters);
        }

        /// <summary>
        /// Details for MixParlay for master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossParlay_MAg(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, int bettype)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);
            parameters.Add("bettype", bettype);
            return ExecSpTable("Age_BetList_SelWinlossParlay_MAg", parameters);
        }

        /// <summary>
        /// Details for MixParlay for super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossParlay_SAg(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, int bettype)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);
            parameters.Add("bettype", bettype);
            return ExecSpTable("Age_BetList_SelWinlossParlay_SAg", parameters);
        }

        #endregion ParlayWinloss

        #region MixParlay

        /// <summary>
        /// Get Mix Parlay BetList
        /// </summary>
        /// <param name="transid"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelParlayDetail(long transid, string language)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("transid", transid);
            parameters.Add("language", language);

            return ExecSpTable("Age_BetList_SelParlayDetail", parameters);
        }

        /// <summary>
        /// Get Combination Mix Parlay BetList Detail
        /// </summary>
        /// <param name="refno"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DbResult Acc_BetlistCombinationParlay_Details(long refno, string language, long transid, long betid, int custId, DateTime balanceupday, DateTime movebetday)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("refno", refno);
            parameters.Add("language", language);
            parameters.Add("transid", transid);
            parameters.Add("betid", betid);
            parameters.Add("custid", custId);
            parameters.Add("movebetday", movebetday);
            parameters.Add("balanceupday", balanceupday);

            return ExecSp("Acc_BetlistCombinationParlay_Details", parameters);
        }

        #endregion MixParlay

        #region BetSlip

        public DataTable Age_BettransSelBetSlip(long transId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("transid", transId);
            parameters.Add("language", language);
            return ExecSpTable("Age_BettransSelBetSlip", parameters);
        }

        public DataTable Age_BettransSelBetSlip_Old(long transId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("transid", transId);
            parameters.Add("language", language);
            return ExecSpTable("Age_BettransSelBetSlip_Old", parameters);
        }

        #endregion BetSlip

        #region Cancel Betlist

        public DataTable Age_BetList_SelDeleted_UserList_Ag(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            return ExecSpTable("Age_BetList_SelDeleted_UserList_Ag", parameters);
        }

        public DataTable Age_BetList_SelDeleted_UserList_MAg(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            return ExecSpTable("Age_BetList_SelDeleted_UserList_MAg", parameters);
        }

        public DataTable Age_BetList_SelDeleted_UserList_SAg(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            return ExecSpTable("Age_BetList_SelDeleted_UserList_SAg", parameters);
        }

        #endregion Cancel Betlist
    }
}