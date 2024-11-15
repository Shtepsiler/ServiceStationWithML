using JOBS.BLL.DTOs.Requests;
using JOBS.BLL.DTOs.Respponces;
using JOBS.BLL.Operations.Mechanics.Commands;
using JOBS.BLL.Operations.Mechanics.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace JOBS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MecahicController : Controller
    {
        private IMediator Mediator;

        public MecahicController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MechanicRequest>>> GetAllAsync()
        {
            try
            {
                 var  List = (List<MechanicRequest>)await Mediator.Send(new MechanicsQuery()); 
                return Ok(List);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateMechanicCommand comand)
        {
            try
            {
                await Mediator.Send(comand);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }






    }
}
