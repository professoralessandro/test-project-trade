#region IMPORT
using AutoMapper;
using Test.Trade.Application.Dtos.Trade.Request;
using Test.Trade.Application.Dtos.Trade.Response;
#endregion

namespace Test.Trade.Infra.Mappers
{
    public class MapperDtoToEntity : Profile
    {
        public MapperDtoToEntity()
        {
            #region TRADE
            CreateMap<TradeResponseDto, Test.Trade.Domain.Entities.Trade>()
            .ReverseMap();

            CreateMap<TradeUpdateRequestDto, Test.Trade.Domain.Entities.Trade>()
            .ReverseMap();

            CreateMap<TradeRegisterRequestDto, Test.Trade.Domain.Entities.Trade>()
             .ReverseMap();
            #endregion TRADE
        }
    }
}
