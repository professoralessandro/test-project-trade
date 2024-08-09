#region IMPORTS
using AppMktPlaceV2.Start.Application.Dtos.Trade.Request;
using AppMktPlaceV2.Start.Application.Dtos.Trade.Response;
using AppMktPlaceV2.Start.Domain.Entities;
using AutoMapper;
#endregion

namespace AppMktPlaceV2.Start.Infrastructure.Mappers
{
    public class MapperEntityToDto : Profile
    {
        public MapperEntityToDto()
        {
            #region TRADE
            CreateMap<Trade, TradeRegisterRequestDto>();

            CreateMap<Trade, TradeUpdateRequestDto>();

            CreateMap<Trade, TradeResponseDto>()
             .ReverseMap();
            #endregion TRADE
        }
    }
}
