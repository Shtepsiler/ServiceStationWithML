using JOBS.BLL.DTOs.Respponces;
using JOBS.BLL.Operations.MechanicsTasks.Commands;
using JOBS.BLL.Operations.MechanicsTasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;

namespace JOBS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Mechanic, User")]
    [Route("api/[controller]")]
    [ApiController]
    public class MechanicsTasksController : ControllerBase
    {
        public IMediator Mediator { get; }
        private readonly IDistributedCache distributedCache;
        public MechanicsTasksController(IMediator mediator, IDistributedCache distributedCache)
        {
            Mediator = mediator;
            this.distributedCache = distributedCache;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await Mediator.Send(new DeleteMechanicTaskCommand() { Id = id });
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateMechanicTaskCommand comand)
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MechanicsTasksDTO>>> GetAllAsync()
        {
            try
            {
                var cacheKey = "TaskList";
                string serializedList;
                var List = new List<MechanicsTasksDTO>();
                var redisList = await distributedCache.GetAsync(cacheKey);
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    List = JsonConvert.DeserializeObject<List<MechanicsTasksDTO>>(serializedList);
                }
                else
                {
                    List = (List<MechanicsTasksDTO>)await Mediator.Send(new GetMechanicsTasksQuery());
                    serializedList = JsonConvert.SerializeObject(List);
                    redisList = Encoding.UTF8.GetBytes(serializedList);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddSeconds(10))
                        .SetSlidingExpiration(TimeSpan.FromSeconds(1));
                    await distributedCache.SetAsync(cacheKey, redisList, options);
                }
                return Ok(List);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetTasksByJobId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MechanicsTasksDTO>>> GetTasksByJobIdAsync([FromQuery] Guid Id)
        {
            try
            {
                var List = (List<MechanicsTasksDTO>)await Mediator.Send(new GetMechanicTaskByJobIdQuery() { Id = Id });
                return Ok(List);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetTasksByMechanicId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MechanicsTasksDTO>>> GetTasksByMechanicAsync([FromQuery] Guid Id)
        {
            try
            {
                var List = (List<MechanicsTasksDTO>)await Mediator.Send(new GetMechanicTaskByMechanicIdQuery() { Id = Id });
                return Ok(List);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MechanicsTasksDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var results = await Mediator.Send(new GetMechanicTaskByIdQuery() { Id = id });
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(UpdateMechanicTaskCommand comand)
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


        [HttpPut("updateStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStatus(UpdateMechanicTaskStatusCommand comand)
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
