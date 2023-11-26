using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParrotIncSquawk.Entities;
using ParrotIncSquawk.Models;
using ParrotIncSquawk.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ParrotIncSquawk.Controllers
{
    [ApiController]
    [Route("api")]
    public class SquawkController : ControllerBase
    {
        private readonly ILogger<SquawkController> _logger;
        private readonly ISquawkService _squawkService;

        public SquawkController(ILogger<SquawkController> logger,
            ISquawkService squawkService)
        {
            _logger = logger;
            _squawkService = squawkService;
        }

        /// <summary>
        /// Controllers should never use auth. Please refer to the documentation provided. 
        /// </summary>
        /// <param name="userId"> The userId is always provided by the API customers</param>
        /// <returns></returns>
        [HttpGet("{userId:guid}/[controller]")]
        [ProducesResponseType(typeof(SquawkRequest), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute, Required] Guid userId,
            [FromRoute, Required] Guid squawkId,
            CancellationToken cancellationToken)
        {
            return Ok(await _squawkService.GetById(userId, squawkId, cancellationToken));
        }

        /// <summary>
        /// Returns a list of Squawks
        /// </summary>
        /// <returns></returns>
        [HttpGet("{userId:guid}/[controller]/{squawkId:guid}")]
        [ProducesResponseType(typeof(IReadOnlyList<Squawk>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            IEnumerable<Squawk> result = await _squawkService.GetAll(cancellationToken);

            if (!result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// This method inserts a new one squawk
        /// </summary>
        /// <param name="squawk"></param>
        /// <returns></returns>
        [HttpPost("{userId:guid}/[controller]")]
        [ProducesResponseType(typeof(Squawk), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> PostAsync(
            [FromRoute, Required] Guid userId,
            [FromBody, Required] SquawkRequest model,
            CancellationToken cancellationToken)
        {
            Ardalis.Result.Result<Squawk> result = await _squawkService.Create(userId, model, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
