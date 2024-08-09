#region ATRIBUTTES
using Dapper;
using Test.Trade.Application.Helper.Static.Generic;
using Test.Trade.Infra.Repositorys.Base;
using Test.Trade.Domain.Interfaces.Repository.Trade;
using Test.Trade.Domain.Connector;
using Test.Trade.Domain.Context.SQLServer;
#endregion

namespace Test.Trade.Infra.Repositorys.Trade
{
    public class TradeRiskRepository : RepositoryBase<Test.Trade.Domain.Entities.Trade>, ITradeRiskRepository
    {
        #region CONSTRUCTOR
        public TradeRiskRepository(AppDbContext context, APConnector session) : base(context, session)
        {
            _context = context;
            _session = session;
        }
        #endregion

        #region RETURN LIST WITH PARAMETERS PAGINATED
        public async Task<IEnumerable<T>> ReturnListWithParametersPaginated<T>(Guid? tradeId = null, string? clientSector = null, string? clientRisk = null, int? pageNumber = null, int? rowspPage = null)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Id", !tradeId.HasValue ? null : tradeId);
            parameters.Add("@ClientSector", clientSector == null || string.IsNullOrEmpty(clientSector) ? null : clientSector.RemoveInjections());
            parameters.Add("@ClientRisk", clientRisk == null || string.IsNullOrEmpty(clientRisk) ? null : clientRisk.RemoveInjections());
            parameters.Add("@PageNumber", pageNumber.HasValue ? pageNumber.Value : 1);
            parameters.Add("@RowspPage", rowspPage.HasValue ? rowspPage.Value : 10);

            var storedProcedure = "[dbo].[ReturnTradePaginated] @Id, @ClientSector, @ClientRisc, @PageNumber, @RowspPage";

            return await ReturnListFromQueryAsync<T>(storedProcedure, parameters);
        }
        #endregion

        #region FIND USERS TO SELECT RETURN KEY AND VALUE
        public async Task<IEnumerable<T>> ReturnUsersToSelectAsync<T>(string? param = null, int? pageNumber = null, int? rowspPage = null)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Param", param == null || string.IsNullOrEmpty(param) ? null : param.RemoveInjections());
            parameters.Add("@PageNumber", pageNumber.HasValue ? pageNumber.Value : 1);
            parameters.Add("@RowspPage", rowspPage.HasValue ? rowspPage.Value : 10);

            var storedProcedure = $@"[seg].[ReturnUsersToSelect] @Param, @PageNumber,@RowspPage";

            return await ReturnListFromQueryAsync<T>(storedProcedure, parameters);
        }
        #endregion
    }
}
