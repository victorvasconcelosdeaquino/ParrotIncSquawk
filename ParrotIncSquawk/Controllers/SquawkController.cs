using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParrotIncSquawk.Entities;
using ParrotIncSquawk.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ParrotIncSquawk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SquawkController : ControllerBase
    {
        private readonly ILogger<SquawkController> _logger;
        private readonly ISquawkService _squawkService;

        public SquawkController(ILogger<SquawkController> logger,
            SquawkService squawkService)
        {
            _logger = logger;
            _squawkService = squawkService;
        }

        /// <summary>
        /// Returns a list of Squawks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Squawk>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _squawkService.GetAll());
        }

        /// <summary>
        /// Controllers should never use auth. Please refer to the documentation provided. 
        /// </summary>
        /// <param name="userId"> The userId is always provided by the API customers</param>
        /// <returns></returns>
        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(Squawk), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid userId)
        {
            return Ok(await _squawkService.GetById(userId));
        }

        /// <summary>
        /// This method inserts a new one squawk
        /// </summary>
        /// <param name="squawk"></param>
        /// <returns></returns>
        [HttpPost/*("{userId:guid}")*/]
        [ProducesResponseType(typeof(Squawk), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> PostAsync(/*
            [FromRoute] Guid userId,*/
            [FromBody] Squawk squawk)
        {
            Guid userId = new Guid();
            return Ok(await _squawkService.Create(userId, squawk));
        }
    }
}
