using MedicineMarketPlace.Admin.Application.Models;
using MedicineMarketPlace.Admin.Application.Services;
using MedicineMarketPlace.Admin.Domain.Constants;
using MedicineMarketPlace.BuildingBlocks.Api.Controllers;
using MedicineMarketPlace.BuildingBlocks.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MedicineMarketPlace.Admin.Api.Controllers
{
    public class TaxStatusController : ApiControllerBase
    {
        private readonly ITaxStatusService _taxStatusService;

        public TaxStatusController(ITaxStatusService taxStatusService)
        {
            _taxStatusService = taxStatusService;
        }

        /// <summary>
        /// Retrive the all tax status details.
        /// </summary>
        /// <returns>A <see cref="IActionResult"/> representing the asynchronous operation.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TaxStatusDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var response = await _taxStatusService.FindAsync();
            return Ok(new ApiResponse((int)HttpStatusCode.OK, CommonMessages.TaxStatuses, response));
        }

        /// <summary>
        /// Retrive the tax status details.
        /// </summary>
        /// <param name="id">Pass the tax status id. </param>
        /// <returns>A <see cref="IActionResult"/> representing the asynchronous operation.</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(TaxStatusDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _taxStatusService.FindByIdAsync(id);
            if (response == null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, CommonMessages.TaxStatusNotFound));

            return Ok(new ApiResponse((int)HttpStatusCode.OK, CommonMessages.TaxStatus, response));
        }

        /// <summary>
        /// Add the tax status details.
        /// </summary>        
        /// <param name="dto"> Pass the tax status details. </param>        
        /// <returns>A <see cref="IActionResult"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TaxStatusDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Post([FromBody] CreateOrUpdateTaxStatusDto dto)
        {
            var response = await _taxStatusService.CreateAsync(dto);
            if (response == null)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.SomethingWentWrong));

            return Ok(new ApiResponse((int)HttpStatusCode.OK, CommonMessages.CreateTaxStatus, response));
        }
    }
}
