#region ATRIBUTTES
using Dapper;
using Test.Trade.Application.Helper.Static.Generic;
using Test.Trade.Infra.Repositorys.Base;
using Test.Trade.Domain.Interfaces.Repository.Trade;
using Test.Trade.Domain.Connector;
using Test.Trade.Domain.Context.SQLServer;
using Test.Trade.Application.Dtos.Trade.Request;
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

            parameters.Add("@TradeId", !tradeId.HasValue ? null : tradeId);
            parameters.Add("@ClientSector", clientSector == null || string.IsNullOrEmpty(clientSector) ? null : clientSector.RemoveInjections());
            parameters.Add("@ClientRisk", clientRisk == null || string.IsNullOrEmpty(clientRisk) ? null : clientRisk.RemoveInjections());
            parameters.Add("@PageNumber", pageNumber.HasValue ? pageNumber.Value : 1);
            parameters.Add("@RowspPage", rowspPage.HasValue ? rowspPage.Value : 10);

            var storedProcedure = "[dbo].[TradePaginated] @TradeId, @ClientSector, @ClientRisk, @PageNumber, @RowspPage";

            return await ReturnListFromQueryAsync<T>(storedProcedure, parameters);
        }
        #endregion

        #region INSERT
        public async Task<Test.Trade.Domain.Entities.Trade> AddAsync(TradeRegisterRequestDto model)
        {
            var parameters = new DynamicParameters();

            var trade = CreateTradeInsertEntity(model);

            parameters.Add("@TradeId", trade.TradeId);
            parameters.Add("@ClientSector", trade.ClientSector);
            parameters.Add("@ClientRisk", trade.ClientRisk);
            parameters.Add("@Value", trade.Value);
            parameters.Add("@DateRegistered", trade.DateRegistered);

            var storedProcedure = "[dbo].[InsertTrade] @TradeId, @Value, @ClientSector, @ClientRisk, @DateRegistered";

            await ExecuteQueryAsync(storedProcedure, parameters);

            return trade;
        }
        #endregion INSERT

        #region UPDATE
        public async Task<Test.Trade.Domain.Entities.Trade> UpdateAsync(TradeUpdateRequestDto model)
        {
            var parameters = new DynamicParameters();

            var trade = CreateTradeUpdatetEntity(model);

            parameters.Add("@TradeId", trade.TradeId);
            parameters.Add("@ClientSector", trade.ClientSector);
            parameters.Add("@ClientRisk", trade.ClientRisk);
            parameters.Add("@Value", trade.Value);
            parameters.Add("@DateUpdated", trade.DateUpdated);

            var storedProcedure = "[dbo].[UpdateTrade] @TradeId, @Value, @ClientSector, @ClientRisk, @DateUpdated";

            await ExecuteQueryAsync(storedProcedure, parameters);

            return trade;
        }
        #endregion UPDATE

        #region DELETE
        public async Task RemoveAsync(TradeDeleteRequestDto model)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@TradeId", model.TradeId);

            var storedProcedure = "[dbo].[DeleteTrade] @TradeId";

            await ExecuteQueryAsync(storedProcedure, parameters);
        }
        #endregion

        #region PRIVATE METHOD
        private string AssessTradeRisk(int value, string clientSector)
        {
            if (value < 1000000 && clientSector.ToUpper() == "Public".ToUpper())
            {
                return "LOWRISK";
            }
            else if (value > 1000000 && clientSector.ToUpper() == "Public".ToUpper())
            {
                return "MEDIUMRISK";
            }
            else if (value > 1000000 && clientSector.ToUpper() == "Private".ToUpper())
            {
                return "HIGHRISK";
            }
            else
            {
                return "UNKNOWN";
            }
        }

        private Test.Trade.Domain.Entities.Trade CreateTradeInsertEntity(TradeRegisterRequestDto model)
        {
            return new Test.Trade.Domain.Entities.Trade { 
                TradeId = Guid.NewGuid(),
                Value = model.Value,
                ClientSector = model.ClientSector,
                ClientRisk = AssessTradeRisk(model.Value, model.ClientSector),
                DateRegistered = DateTime.Now
            };
        }

        private Test.Trade.Domain.Entities.Trade CreateTradeUpdatetEntity(TradeUpdateRequestDto model)
        {
            return new Test.Trade.Domain.Entities.Trade
            {
                TradeId = model.TradeId,
                Value = model.Value,
                ClientSector = model.ClientSector,
                ClientRisk = AssessTradeRisk(model.Value, model.ClientSector),
                DateUpdated = DateTime.Now
            };
        }
        #endregion
    }
}
