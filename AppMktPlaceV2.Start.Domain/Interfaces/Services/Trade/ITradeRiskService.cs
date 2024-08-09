#region IMPORTS
using Test.Trade.Application.Dtos.Trade.Request;
using Test.Trade.Application.Dtos.Trade.Response;
#endregion IMPORTS

namespace Test.Trade.Domain.Interfaces.Services.Trade
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
        Task<Entities.Trade> InsertAsync(TradeRegisterRequestDto model);
        #endregion

        #region UPDATE
        Task<Entities.Trade> UpdateAsync(TradeUpdateRequestDto model);
        #endregion

        #region DELETE SERVIÇO DE DELETE
        Task DeleteAsync(Guid tradeId);
        #endregion
    }
}
