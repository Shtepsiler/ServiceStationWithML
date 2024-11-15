using JOBS.BLL.Common.Validation;
using JOBS.BLL.DTOs.Respponces;
using JOBS.BLL.Operations.Jobs.Commands;
using JOBS.BLL.Operations.Jobs.Queries;
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
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private IMediator Mediator;
        private UpdateJobCommandValidator UdateJobCommandValidator;
        private CreateJobCommandValidator CreateJobCommandValidator;
        private readonly IDistributedCache distributedCache;

        public JobController(IMediator mediator, UpdateJobCommandValidator updateJobCommandValidator, CreateJobCommandValidator createJobCommandValidator, IDistributedCache distributedCache)
        {
            Mediator = mediator;
            UdateJobCommandValidator = updateJobCommandValidator;
            CreateJobCommandValidator = createJobCommandValidator;
            this.distributedCache = distributedCache;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await Mediator.Send(new DeleteJobCommand { Id = id });
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechainc,User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateJobCommand comand)
        {
            try
            {
                var isValid = CreateJobCommandValidator.Validate(comand);
                if (isValid.IsValid)
                {
                    await Mediator.Send(comand);
                    return Ok();
                }
                else
                {
                    return ValidationProblem(isValid.Errors.ToString());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechanic")]
        [HttpPost("AddOrderToJob")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrderToJobAsync([FromQuery]AddOrderToJobCommand comand)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    await Mediator.Send(comand);
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechanic")]
        [HttpPut("updateMechanic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateJobsMechanicAsync([FromQuery] UpdateJobsMechanicCommand comand)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    await Mediator.Send(comand);
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechanic")]
        [HttpPut("updateStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStatusAsync([FromQuery] UpdateJobStatusCommand comand)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    await Mediator.Send(comand);
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechanic")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetAllAsync()
        {
            try
            {
                var cacheKey = "JobList";
                string serializedList;
                var List = new List<JobDTO>();
                var redisList = await distributedCache.GetAsync(cacheKey);
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    List = JsonConvert.DeserializeObject<List<JobDTO>>(serializedList);
                }
                else
                {
                    List = (List<JobDTO>)await Mediator.Send(new GetJobsQuery());
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Mechanic")]
        [HttpGet("GetJobByMechanicId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJobByMechanicIdAsync([FromQuery] Guid Id)
        {
            try
            {
                var List = (List<JobDTO>)await Mediator.Send(new GetJobsByMechanicIdQuery() { MecchanicId = Id });
                return Ok(List);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Mechanic, User")]
        [HttpGet("GetJobsBYUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJobsBYUserIdAsync(Guid Id)
        {
            try
            {
                var List = (List<JobDTO>)await Mediator.Send(new GetJobsByUserIdQuery() { UserId = Id });
                return Ok(List);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Mechanic, User")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JobWithTasksDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var results = await Mediator.Send(new GetJobByIdQuery() { Id = id });
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechanic,User")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(UpdateJobCommand comand)
        {
            try
            {
                var isValid = UdateJobCommandValidator.Validate(comand);
                if (isValid.IsValid)
                {
                    await Mediator.Send(comand);
                    return Ok();
                }
                else
                {
                    return ValidationProblem(isValid.Errors.ToString());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechanic,User")]
        [HttpPut("updateModelAproved")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateModelAproved(UpdateModelApproved comand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await Mediator.Send(comand);
                    return Ok();
                }
                else
                {
                    return ValidationProblem(ModelState.ToString());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("uncertain-samples")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUncertainSamplesAsync([FromQuery] float confidence, bool choseApproverd)
        {
            try
            {
                // Викликаємо запит через Mediator
                var result = await Mediator.Send(new GetUncertainSamplesQuery { Confidence = confidence, ChoseApproverd = choseApproverd});

                // Перевіряємо, чи були знайдені дані
                if (result == null || !result.new_data.Any())
                {
                    return NotFound("No uncertain samples found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
