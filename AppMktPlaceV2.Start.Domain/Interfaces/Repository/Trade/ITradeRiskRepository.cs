namespace AppMktPlaceV2.Start.Domain.Interfaces.Repository.Trade
{
    public interface ITradeRiskRepository
    {
        #region FIND BY ID
        Task<AppMktPlaceV2.Start.Domain.Entities.Trade> GetByIdAsync(Guid id);
        #endregion

        #region RETURN LIST WITH PARAMETERS PAGINATED
        Task<IEnumerable<T>> ReturnListFromQueryAsync<T>(string query, object? param = null);
        #endregion

        #region RETURN SINGLE WITH PARAMETERS PAGINATED
        Task<IEnumerable<T>> ReturnListWithParametersPaginated<T>(Guid? tradeId = null, string? clientSector = null, string? clientRisk = null, int? pageNumber = null, int? rowspPage = null);
        #endregion

        #region GET ALL
        Task<IEnumerable<AppMktPlaceV2.Start.Domain.Entities.Trade>> GetAllAsync();
        #endregion

        #region INSERT
        Task AddAsync(AppMktPlaceV2.Start.Domain.Entities.Trade obj);
        #endregion

        #region UPDATE
        Task UpdateAsync(AppMktPlaceV2.Start.Domain.Entities.Trade obj);
        #endregion

        #region DELETE
        Task RemoveAsync(AppMktPlaceV2.Start.Domain.Entities.Trade obj);
        #endregion
    }
}
