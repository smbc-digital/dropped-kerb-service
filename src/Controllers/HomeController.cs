using dropped_kerb_service.Models;
using dropped_kerb_service.Services;
using Microsoft.AspNetCore.Mvc;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;
using StockportGovUK.AspNetCore.Availability.Managers;
using System;
using System.Threading.Tasks;

namespace dropped_kerb_service.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[Controller]")]
    [ApiController]
    [TokenAuthentication]
    public class HomeController : ControllerBase
    {
        //private IAvailabilityManager _availabilityManager;
        private readonly IDroppedKerbService _droppedKerbService;

        public HomeController(IDroppedKerbService droppedKerbService)
        {
            // _availabilityManager = availabilityManager;
            _droppedKerbService = droppedKerbService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DroppedKerbRequest droppedKerbRequest)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Invalid request parameters.");

            //return Ok();

            return Ok(await _droppedKerbService.CreateCase(droppedKerbRequest));
        }
    }
}