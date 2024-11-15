using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using PARTS.BLL.DTOs.Responses;
using Newtonsoft.Json;
using PARTS.DAL.Interfaces;
using PARTS.BLL.Services.Interaces;
using PARTS.BLL.DTOs.Requests;


namespace ClientPartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    { 
        private readonly ILogger<VehicleController> _logger;
        private readonly IDistributedCache distributedCache;
        private readonly IVehicleService vehicleService;
        public VehicleController(
            ILogger<VehicleController> logger,
             IDistributedCache distributedCache
,
             IVehicleService VehicleService
             )
        {
            _logger = logger;
            this.distributedCache = distributedCache;
            this.vehicleService = VehicleService;
        }

      //  [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleResponse>>> GetAllAsync()
        {
            try
            {

                var cacheKey = "VehicleList";
                string serializedList;
                var List = new List<VehicleResponse>();
                byte[]? redisList = null;
                try
                {
                    redisList = await distributedCache.GetAsync(cacheKey);

                }
                catch (Exception e) 
                {
                    _logger.LogWarning($"Failed to retrieve data from cache: {e.Message}");
                }
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    List = JsonConvert.DeserializeObject<List<VehicleResponse>>(serializedList);
                }
                else
                {
                    List = (List<VehicleResponse>)await vehicleService.GetAllAsync();
                    try{
                        serializedList = JsonConvert.SerializeObject(List);
                        redisList = Encoding.UTF8.GetBytes(serializedList);
                        var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                        await distributedCache.SetAsync(cacheKey, redisList, options);
                    }
                    catch(Exception e)
                    {
                     _logger.LogWarning($"Failed to set data to cache: {e.Message}");
                    }

                }
                _logger.LogInformation($"VehicleController            GetAllAsync");
                return Ok(List);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

      //  [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult<VehicleResponse>> GetByIdAsync(Guid Id)
        {
            try
            {
                var result = await vehicleService.GetByIdAsync(Id);
                if (result == null)
                {
                    _logger.LogInformation($"Vehicle із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали Vehicle з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        //  [Authorize]
        [HttpPost]
        public async Task<ActionResult<VehicleResponse>> PostAsync([FromBody] VehicleRequest req)
        {
            try
            {
                if (req == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Vehicle є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Vehicle є некоректним");
                }
               var res =  await vehicleService.PostAsync(req);


                return Created("",res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі PostAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

      
      //  [Authorize]
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] VehicleRequest req)
        {
            try
            {
                if (req == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Vehicle є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Vehicle є некоректним");
                }

                await vehicleService.UpdateAsync(req);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі UpdateAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

     //   [Authorize]
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteByIdAsync(Guid id)
        {
            try
            {
                var event_entity = await vehicleService.GetByIdAsync(id);
                if (event_entity == null)
                {
                    _logger.LogInformation($"Запис Vehicle із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await vehicleService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("new")]
        public async Task<ActionResult<VehicleResponse>> PostVehicleAsync([FromBody] CreateVehicleRequest vehicleRequest)
        {
            try
            {
                if (vehicleRequest == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Vehicle є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Vehicle є некоректним");
                }

                // Create the vehicle via service
                var vehicleId = await vehicleService.CreateVehicle(vehicleRequest);

                // Return response with the created vehicle ID
                return Ok(vehicleId.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі PostAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
