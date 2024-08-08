#region IMPORT
using AppMktPlaceV2.Start.Application.Dtos.Trade.Request;
using AppMktPlaceV2.Start.Application.Dtos.Trade.Response;
using AppMktPlaceV2.Start.Application.Helper.Static.Generic;
using AppMktPlaceV2.Start.Domain.Interfaces.Repository.Trade;
using AppMktPlaceV2.Start.Domain.Interfaces.Services.Trade;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
#endregion IMPORT

namespace AppMktPlaceV2.Start.Domain.Servies.Trade
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

                return _mapper.Map<AppMktPlaceV2.Start.Domain.Entities.Trade, TradeResponseDto>(trade);
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

                return _mapper.Map<IEnumerable<AppMktPlaceV2.Start.Domain.Entities.Trade>, IEnumerable<TradeResponseDto>>(result);
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
        public async Task<string> InsertAsync(TradeRegisterRequestDto model)
        {
            try
            {

                var resultDto = _mapper.Map<TradeRegisterRequestDto, AppMktPlaceV2.Start.Domain.Entities.Trade>(model.TrasnformObjectPropValueToUpper());

                resultDto.DateRegistered = DateTime.Now;
                resultDto.ClientRisk = AssessTradeRisk(model);

                await _repository.AddAsync(resultDto);

                return resultDto.ClientRisk;
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
        public async Task<string> UpdateAsync(TradeRegisterRequestDto model)
        {
            try
            {

                var trade = _mapper.Map<TradeRegisterRequestDto, AppMktPlaceV2.Start.Domain.Entities.Trade>(model.TrasnformObjectPropValueToUpper());

                trade.DateUpdated = DateTime.Now;
                trade.ClientRisk = AssessTradeRisk(model);

                await _repository.UpdateAsync(trade);

                return trade.ClientRisk;
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
        public async Task DeleteAsync(Guid tradeId)
        {
            try
            {
                var userFromDB = await _repository.GetByIdAsync(tradeId);

                await _repository.RemoveAsync(userFromDB);
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
        private string AssessTradeRisk(TradeRegisterRequestDto trade)
        {
            if (trade.Value < 1000000 && trade.ClientSector == "Public")
            {
                return "LOWRISK";
            }
            else if (trade.Value > 1000000 && trade.ClientSector == "Public")
            {
                return "MEDIUMRISK";
            }
            else if (trade.Value > 1000000 && trade.ClientSector == "Private")
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
