using FccuOpsLite.Models.Api;
using FccuOpsLite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FccuOpsLite.Controllers.Api
{
    [ApiController]
    [Route("api/loanapplications")]
    [Authorize(Policy = "CanReadLoanData")]
    [Produces("application/json", "application/xml")]
    public class LoanApplicationsApiController : ControllerBase
    {
        private readonly ILoanApiService _loanApiService;

        public LoanApplicationsApiController(ILoanApiService loanApiService)
        {
            _loanApiService = loanApiService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LoanApplicationSummaryApiDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult<IEnumerable<LoanApplicationSummaryApiDto>>> GetLoanApplications(
            [FromQuery] LoanApplicationQueryApiDto query)
        {
            if (query.MinAmount.HasValue && query.MaxAmount.HasValue && query.MinAmount > query.MaxAmount)
            {
                ModelState.AddModelError(nameof(query.MinAmount), "MinAmount cannot be greater than MaxAmount.");
                return ValidationProblem(ModelState);
            }

            var results = await _loanApiService.GetLoanApplicationsAsync(query);
            return Ok(results);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LoanApplicationDetailsApiDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult<LoanApplicationDetailsApiDto>> GetLoanApplicationById(int id)
        {
            var result = await _loanApiService.GetLoanApplicationByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}