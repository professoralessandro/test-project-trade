#region IMPORTS
using AppMktPlaceV2.Start.Application.Dtos.Trade.Request;
using AppMktPlaceV2.Start.Application.Dtos.Trade.Response;
#endregion IMPORTS

namespace AppMktPlaceV2.Start.Domain.Interfaces.Services.Trade
{
    public interface ITradeRiskService
    {
        #region FIND BY ID
        Task<TradeResponseDto> GetByIdAsync(Guid userId);
        #endregion

        #region RETURN LIST WITH PARAMETERS PAGINATED
        Task<IEnumerable<TradeResponseDto>> ReturnListWithParametersPaginated(Guid? tradeId = null, string? clientSector = null, string? clientRisk = null, int? pageNumber = null, int? rowspPage = null);
        #endregion

        #region GET ALL ASYNC
        Task<IEnumerable<TradeResponseDto>> GetAllAsync();
        #endregion

        #region INSERT
        Task<string> InsertAsync(TradeRegisterRequestDto model);
        #endregion

        #region UPDATE
        Task<string> UpdateAsync(TradeRegisterRequestDto model);
        #endregion

        #region DELETE SERVIÇO DE DELETE
        Task DeleteAsync(Guid tradeId);
        #endregion
    }
}
