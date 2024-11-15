using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MediatR;
using JOBS.BLL.DTOs.Respponces;
using JOBS.BLL.Operations.Jobs.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;
using JOBS.BLL.Operations.Specialisation.Queries;

namespace JOBS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialisationController : Controller
    {
        private IMediator Mediator;
        private readonly IDistributedCache distributedCache;

        public SpecialisationController(IMediator mediator, IDistributedCache distributedCache)
        {
            Mediator = mediator;
            this.distributedCache = distributedCache;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechanic")]
        [HttpGet("brief")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SpecialisationDTO>>> GetSpecialisationsAsync()
        {
            try
            {
                var cacheKey = "SpecialisationList";
                string serializedList;
                var List = new List<SpecialisationDTO>();
                var redisList = await distributedCache.GetAsync(cacheKey);
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    List = JsonConvert.DeserializeObject<List<SpecialisationDTO>>(serializedList);
                }
                else
                {
                    List = (List<SpecialisationDTO>)await Mediator.Send(new GetSpecialisationsQuery());
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

    }
}
