#region IMPORT
using AppMktPlaceV2.Start.Application.Dtos.Trade.Request;
using AppMktPlaceV2.Start.Application.Dtos.Trade.Response;
using AppMktPlaceV2.Start.Domain.Entities;
using AutoMapper;
#endregion

namespace AppMktPlaceV2.Start.Infrastructure.Mappers
{
    public class MapperDtoToEntity : Profile
    {
        public MapperDtoToEntity()
        {
            #region TRADE
            CreateMap<TradeResponseDto, Trade>()
            .ReverseMap();

            CreateMap<TradeRegisterRequestDto, Trade>()
             .ReverseMap();
            #endregion TRADE
        }
    }
}
