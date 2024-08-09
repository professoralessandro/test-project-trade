using AppMktPlaceV2.Application.Dtos;
using Test.Trade.Business.Entities;
using Test.Trade.Business.Interfaces.Services.Resource;

namespace Test.Trade.Business.Business.Resource
{
    public static class ResourceValdation
    {
        #region INSERT
        public static async Task<string> ValidInsert(this ResourceDto model, IResourceService _service)
        {
            return string.Empty;
        }
        #endregion

        #region UPDATE
        public static string ValidUpdate(this ResourceDto model)
        {
            return string.Empty;
        }
        #endregion

        #region DELETE
        public static string ValidDelete(this Recurso model)
        {
            return string.Empty;
        }
        #endregion
    }
}
