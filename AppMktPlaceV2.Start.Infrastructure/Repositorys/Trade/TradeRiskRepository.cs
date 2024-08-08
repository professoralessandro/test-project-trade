#region ATRIBUTTES
using AppMktPlaceV2.Start.Infrastructure.Repositorys.Base;
using AppMktPlaceV2.Start.Application.Helper.Static.Generic;
using AppMktPlaceV2.Start.Domain.Context.SQLServer;
using AppMktPlaceV2.Start.Domain.Entities;
using Dapper;
using AppMktPlaceV2.Start.Domain.Connector;
using AppMktPlaceV2.Start.Domain.Interfaces.Repository.Trade;
#endregion

namespace AppMktPlaceV2.Start.Infrastructure.Repositorys.User
{
    public class TradeRiskRepository : RepositoryBase<Trade>, ITradeRiskRepository
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

            return await this.ReturnListFromQueryAsync<T>(storedProcedure, parameters);
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

            return await this.ReturnListFromQueryAsync<T>(storedProcedure, parameters);
        }
        #endregion
    }
}
