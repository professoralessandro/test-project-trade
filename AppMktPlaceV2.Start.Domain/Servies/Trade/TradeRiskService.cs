﻿#region IMPORT
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
        public async Task<AppMktPlaceV2.Start.Domain.Entities.Trade> InsertAsync(TradeRegisterRequestDto model)
        {
            try
            {
                var trade = _mapper.Map<TradeRegisterRequestDto, AppMktPlaceV2.Start.Domain.Entities.Trade>(model.TrasnformObjectPropValueToUpper());

                trade.DateRegistered = DateTime.Now;
                trade.ClientRisk = AssessTradeRisk(model.Value, model.ClientSector);

                await _repository.AddAsync(trade);

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
        public async Task<AppMktPlaceV2.Start.Domain.Entities.Trade> UpdateAsync(TradeUpdateRequestDto model)
        {
            try
            {
                var trade = await _repository.GetByIdAsync(model.TradeId);

                if (trade == null) throw new ValidationException("Houve um erro ao buscar o registro desejado!");

                trade.DateUpdated = DateTime.Now;
                trade.ClientRisk = AssessTradeRisk(model.Value, model.ClientSector);

                await _repository.UpdateAsync(trade);

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
