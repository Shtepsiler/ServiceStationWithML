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
using ClientPartAPI.Helpers;
using PARTS.BLL.Services;


namespace ClientPartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakeController : ControllerBase
    { 
        private readonly ILogger<MakeController> _logger;
        private readonly IDistributedCache distributedCache;
        private readonly IMakeService makeService;
        public MakeController(
            ILogger<MakeController> logger,
             IDistributedCache distributedCache
,
             IMakeService MakeService
             )
        {
            _logger = logger;
            this.distributedCache = distributedCache;
            this.makeService = MakeService;
        }

        //  [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<MakeResponse>>> GetAllAsync([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var cacheKey = $"MakeList_{paginationParams.PageNumber}_{paginationParams.PageSize}";
                string serializedList;
                Pagination<MakeResponse> paginatedList;
                var redisList = await distributedCache.GetAsync(cacheKey);

                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    paginatedList = JsonConvert.DeserializeObject<Pagination<MakeResponse>>(serializedList);
                }
                else
                {
                    var makes = await makeService.GetAllAsync();
                    paginatedList = Pagination<MakeResponse>.ToPagedList(makes, paginationParams.PageNumber, paginationParams.PageSize);

                    serializedList = JsonConvert.SerializeObject(paginatedList);
                    redisList = Encoding.UTF8.GetBytes(serializedList);

                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                    await distributedCache.SetAsync(cacheKey, redisList, options);
                }

                _logger.LogInformation($"MakeController - GetAllAsync with pagination: Page {paginationParams.PageNumber}, Size {paginationParams.PageSize}");
                return Ok(paginatedList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //  [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult<MakeResponse>> GetByIdAsync(Guid Id)
        {
            try
            {
                var result = await makeService.GetByIdAsync(Id);
                if (result == null)
                {
                    _logger.LogInformation($"Make із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали Make з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

       
      //  [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] MakeRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Make є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Make є некоректним");
                }
                await makeService.PostAsync(brand);


                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі PostAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

      
      //  [Authorize]
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] MakeRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Make є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Make є некоректним");
                }

                await makeService.UpdateAsync(brand);
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
                var event_entity = await makeService.GetByIdAsync(id);
                if (event_entity == null)
                {
                    _logger.LogInformation($"Запис Make із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await makeService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        //  [Authorize]
        [HttpGet("titles")]
        public async Task<ActionResult<ChooseMakeResponse>> GetTitles()
        {
            try
            {

                var cacheKey = "MakeList";
                string serializedList;
                var List = new List<ChooseMakeResponse>();
                var redisList = await distributedCache.GetAsync(cacheKey);
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    List = JsonConvert.DeserializeObject<List<ChooseMakeResponse>>(serializedList);
                }
                else
                {
                    List = (List<ChooseMakeResponse>)await makeService.GetMakeTitles();
                    serializedList = JsonConvert.SerializeObject(List);
                    redisList = Encoding.UTF8.GetBytes(serializedList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddHours(1))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    await distributedCache.SetAsync(cacheKey, redisList, options);
                }
                _logger.LogInformation($"MakeController            GetTitles");
                return Ok(List);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Щось пішло не так у методі GetTitles() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
