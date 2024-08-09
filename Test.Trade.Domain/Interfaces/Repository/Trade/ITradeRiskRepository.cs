#region REFERENCE
using Test.Trade.Application.Dtos.Trade.Request;
#endregion REFERENCE

namespace Test.Trade.Domain.Interfaces.Repository.Trade
{
    public interface ITradeRiskRepository
    {
        #region FIND BY ID
        Task<Entities.Trade> GetByIdAsync(Guid id);
        #endregion

        #region RETURN LIST WITH PARAMETERS PAGINATED
        Task<IEnumerable<T>> ReturnListFromQueryAsync<T>(string query, object? param = null);
        #endregion

        #region RETURN SINGLE WITH PARAMETERS PAGINATED
        Task<IEnumerable<T>> ReturnListWithParametersPaginated<T>(Guid? tradeId = null, string? clientSector = null, string? clientRisk = null, int? pageNumber = null, int? rowspPage = null);
        #endregion

        #region GET ALL
        Task<IEnumerable<Entities.Trade>> GetAllAsync();
        #endregion

        #region INSERT
        Task AddAsync(Entities.Trade obj);

        Task<Test.Trade.Domain.Entities.Trade> AddAsync(TradeRegisterRequestDto obj);
        #endregion

        #region UPDATE
        Task UpdateAsync(Entities.Trade obj);

        Task<Test.Trade.Domain.Entities.Trade> UpdateAsync(TradeUpdateRequestDto obj);
        #endregion

        #region DELETE
        Task RemoveAsync(Entities.Trade model);

        Task RemoveAsync(TradeDeleteRequestDto tradeId);
        #endregion
    }
}
