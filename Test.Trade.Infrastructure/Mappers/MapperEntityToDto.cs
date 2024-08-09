#region IMPORTS
using AutoMapper;
using Test.Trade.Application.Dtos.Trade.Request;
using Test.Trade.Application.Dtos.Trade.Response;
#endregion

namespace Test.Trade.Infra.Mappers
{
    public class MapperEntityToDto : Profile
    {
        public MapperEntityToDto()
        {
            #region TRADE
            CreateMap<Test.Trade.Domain.Entities.Trade, TradeRegisterRequestDto>();

            CreateMap<Test.Trade.Domain.Entities.Trade, TradeUpdateRequestDto>();

            CreateMap<Test.Trade.Domain.Entities.Trade, TradeResponseDto>()
             .ReverseMap();
            #endregion TRADE
        }
    }
}
