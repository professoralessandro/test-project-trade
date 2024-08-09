#region IMPORT
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Test.Trade.Application.Dtos.Trade.Request;
using Test.Trade.Application.Dtos.Trade.Response;
using Test.Trade.Application.Helper.Static.Generic;
using Test.Trade.Domain.Interfaces.Repository.Trade;
using Test.Trade.Domain.Interfaces.Services.Trade;
#endregion IMPORT

namespace Test.Trade.Domain.Servies.Trade
{
    public class TradeRiskService : ITradeRiskService
    {
        #region ATRIBUTTES
        private readonly ITradeRiskRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region CONTRUCTORS
        public TradeRiskService(ITradeRiskRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region FIND BY ID
        public async Task<TradeResponseDto> GetByIdAsync(Guid tradeId)
        {
            try
            {
                var trade = await _repository.GetByIdAsync(tradeId);

                if (trade == null) throw new Exception("Trade not found");

                return _mapper.Map<Entities.Trade, TradeResponseDto>(trade);
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar o registro desejado!" + ex.Message);
            }
        }
        #endregion

        #region RETURN LIST WITH PARAMETERS PAGINATED
        public async Task<IEnumerable<TradeResponseDto>> ReturnListWithParametersPaginated(Guid? tradeId = null, string? clientSector = null, string? clientRisk = null, int? pageNumber = null, int? rowspPage = null)
        {
            try
            {
                return await _repository.ReturnListWithParametersPaginated<TradeResponseDto>(tradeId, clientSector, clientRisk, pageNumber, rowspPage);
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar o registro desejado!" + ex.Message);
            }
        }
        #endregion

        #region GETALL
        public async Task<IEnumerable<TradeResponseDto>> GetAllAsync()
        {
            try
            {
                var msgLog = "\n======================================================================================";
                msgLog += "\nBuscando por usuarios na base de dados";
                msgLog += "\n======================================================================================";
                Serilog.Log.Information(msgLog);

                var result = await _repository.GetAllAsync();

                if (result == null) throw new Exception("Trade not found");

                return _mapper.Map<IEnumerable<Entities.Trade>, IEnumerable<TradeResponseDto>>(result);
            }
            catch (Exception ex)
            {
                var msgLog = "\n======================================================================================";
                msgLog += "\nERROR: Não foi possível realizar a busca por registros: \n" + ex.Message;
                msgLog += "\n======================================================================================";
                Serilog.Log.Error(msgLog);
                throw new Exception("Não foi possível realizar a busca por registros: " + ex.Message);
            }
        }
        #endregion

        #region INSERT
        public async Task<Entities.Trade> InsertAsync(TradeRegisterRequestDto model)
        {
            try
            {
                var trade = await _repository.AddAsync(model.TrasnformObjectPropValueToUpper());

                return trade;
            }
            catch (ValidationException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao incluir registro: " + ex.Message);
            }
        }
        #endregion

        #region UPDATE
        public async Task<Entities.Trade> UpdateAsync(TradeUpdateRequestDto model)
        {
            try
            {
                #region VALIDATION
                var trade = await _repository.GetByIdAsync(model.TradeId);

                if (trade == null) throw new ValidationException("Houve um erro ao buscar o registro desejado!");
                #endregion VALIDATION

                trade = await _repository.UpdateAsync(model.TrasnformObjectPropValueToUpper());

                return trade;
            }
            catch (ValidationException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao incluir registro: " + ex.Message);
            }
        }
        #endregion

        #region DELETE
        public async Task DeleteAsync(TradeDeleteRequestDto trade)
        {
            try
            {
                #region VALIDATION
                var tradeFromDB = await _repository.GetByIdAsync(trade.TradeId);

                if (tradeFromDB == null) throw new ValidationException("Houve um erro ao buscar o registro desejado!");
                #endregion VALIDATION

                await _repository.RemoveAsync(trade);
            }
            catch (ValidationException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao tentar editar o registro: " + ex.Message);
            }
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
        #endregion
    }
}
