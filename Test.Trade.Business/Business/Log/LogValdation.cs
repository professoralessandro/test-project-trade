using AppMktPlaceV2.Domain.Interfaces.Services.Log;
using Test.Trade.Application.Dtos.Log;

namespace Test.Trade.Business.Business.Log
{
    public static class LogValdation
    {
        #region INSERT
        public static async Task<string> ValidInsert(this LogDto model, ILogService _service)
        {
            return string.Empty;
        }
        #endregion

        #region UPDATE
        public static string ValidUpdate(this LogDto model)
        {
            return string.Empty;
        }
        #endregion

        #region DELETE
        public static string ValidDelete(this LogDto model)
        {
            return string.Empty;
        }
        #endregion
    }
}
