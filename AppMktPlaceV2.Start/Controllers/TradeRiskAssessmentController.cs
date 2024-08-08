#region IMPORTS
using AppMktPlaceV2.Start.Api.Controllers.Common;
using AppMktPlaceV2.Start.Application.Dtos.Trade.Request;
using AppMktPlaceV2.Start.Application.Dtos.Trade.Response;
using AppMktPlaceV2.Start.Application.Helper.Static.Generic;
using AppMktPlaceV2.Start.Domain.Interfaces.Services.Trade;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
#endregion IMPORTS

namespace AppMktPlaceV2.Start.Api.Controllers
{
    public class TradeRiskAssessmentController : CommonController
    {
        #region ATRIBUTTES
        private readonly IMapper _mapper;
        private readonly ITradeRiskService _service;
        #endregion ATRIBUTTES

        #region CONTRUCTORS
        public TradeRiskAssessmentController(ITradeRiskService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        #endregion CONTRUCTORS

        #region REGISTER
        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Registeruser([FromBody] TradeRegisterRequestDto userObj)
        {
            try
            {
                var result =  await _service.InsertAsync(userObj);

                return Ok(new
                {
                    Risk = result
                });
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, UtilHelper.FormatLogInformationMessage(message: "ERROR => Host terminated unexpectedly", userId: Guid.Parse("d2a833de-5bb4-4931-a3c2-133c8994072a")));
                return BadRequest(ex.Message);
            }
        }
        #endregion REGISTER

        #region GET BY ID
        [HttpGet, Route("GetById"), OutputCache]
        public async Task<ActionResult<TradeResponseDto>> GetById(
            [FromQuery] Guid UserId
            )
        {
            try
            {
                var result = await _service.GetByIdAsync(UserId);

                if (result == null) { return NotFound($"register: {UserId} not found"); }

                Serilog.Log.Information("Operation completed successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Operation ERROR");
                return UnprocessableEntity(ex.Message);
            }
        }
        #endregion

        #region GET ALL TRADES BY PARAMETER PAGINATED
        [HttpGet, Route("get-all-paginated-by-parameter")]
        public async Task<ActionResult<TradeResponseDto>> GetAllTradesByParameter(
                [FromQuery] Guid? tradeId,
                [FromQuery] string? clientSector,
                [FromQuery] string? clientRisk,
                [FromQuery] int? pageNumber,
                [FromQuery] int? rowspPage
            )
        {
            try
            {
                return Ok(await _service.ReturnListWithParametersPaginated(tradeId, clientSector, clientRisk, pageNumber, rowspPage));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, UtilHelper.FormatLogInformationMessage(message: "ERROR => Host terminated unexpectedly", userId: Guid.Parse("d2a833de-5bb4-4931-a3c2-133c8994072a")));
                return BadRequest(ex.Message);
            }
        }
        #endregion GET ALL TRADES BY PARAMETER PAGINATED

        #region GET ALL
        [HttpGet, Route("get-all")]
        public async Task<ActionResult<TradeResponseDto>> GetAllTrades()
        {
            try
            {
                return Ok(await _service.GetAllAsync());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, UtilHelper.FormatLogInformationMessage(message: "ERROR => Host terminated unexpectedly", userId: Guid.Parse("d2a833de-5bb4-4931-a3c2-133c8994072a")));
                return BadRequest(ex.Message);
            }
        }
        #endregion GET ALL

        #region UPDATE
        [HttpPut]
        public async Task<ActionResult<TradeResponseDto>> Update(TradeRegisterRequestDto model)
        {
            try
            {
                if (!Guid.TryParse(this.Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value, out Guid userId)) throw new ValidationException("Error to validate user");

                var response = await _service.UpdateAsync(model);
                Serilog.Log.Information("Operation completed successfully");
                return Ok(new
                {
                    Risk = response
                });
            }
            catch (Exception ex)
            {
                this.Response.StatusCode = 422;
                Serilog.Log.Error(ex, ex.Message);
                return UnprocessableEntity(ex.Message);
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                await _service.DeleteAsync(Id);
                Serilog.Log.Information("Operation completed successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                this.Response.StatusCode = 422;
                Serilog.Log.Error(ex, ex.Message);
                return UnprocessableEntity(ex.Message);
            }
        }
        #endregion
    }
}
