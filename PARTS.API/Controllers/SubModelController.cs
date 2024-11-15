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
using PARTS.BLL.Services;


namespace ClientPartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubModelController : ControllerBase
    { 
        private readonly ILogger<SubModelController> _logger;
        private readonly IDistributedCache distributedCache;
        private readonly ISubModelService subModelService;
        public SubModelController(
            ILogger<SubModelController> logger,
             IDistributedCache distributedCache
,
             ISubModelService SubModelService
             )
        {
            _logger = logger;
            this.distributedCache = distributedCache;
            this.subModelService = SubModelService;
        }

      //  [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubModelResponse>>> GetAllAsync()
        {
            try
            {

                var cacheKey = "SubModelList";
                string serializedList;
                var List = new List<SubModelResponse>();
                var redisList = await distributedCache.GetAsync(cacheKey);
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    List = JsonConvert.DeserializeObject<List<SubModelResponse>>(serializedList);
                }
                else
                {
                    List = (List<SubModelResponse>)await subModelService.GetAllAsync(); 
                    serializedList = JsonConvert.SerializeObject(List);
                    redisList = Encoding.UTF8.GetBytes(serializedList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    await distributedCache.SetAsync(cacheKey, redisList, options);
                }
                _logger.LogInformation($"SubModelController            GetAllAsync");
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
        public async Task<ActionResult<SubModelResponse>> GetByIdAsync(Guid Id)
        {
            try
            {
                var result = await subModelService.GetByIdAsync(Id);
                if (result == null)
                {
                    _logger.LogInformation($"SubModel із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали SubModel з бази даних!");
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
        public async Task<ActionResult> PostAsync([FromBody] SubModelRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт SubModel є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт SubModel є некоректним");
                }
                await subModelService.PostAsync(brand);


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
        public async Task<ActionResult> UpdateAsync([FromBody] SubModelRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт SubModel є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт SubModel є некоректним");
                }

                await subModelService.UpdateAsync(brand);
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
                var event_entity = await subModelService.GetByIdAsync(id);
                if (event_entity == null)
                {
                    _logger.LogInformation($"Запис SubModel із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await subModelService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("titles")]
        public async Task<ActionResult<ChooseSubModeResponse>> GetTitles(Guid Id)
        {
            try
            {

                var cacheKey = $"SubModelList_{Id}";
                string serializedList;
                var List = new List<ChooseSubModeResponse>();
                var redisList = await distributedCache.GetAsync(cacheKey);
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    List = JsonConvert.DeserializeObject<List<ChooseSubModeResponse>>(serializedList);
                }
                else
                {
                    List = (List<ChooseSubModeResponse>)await subModelService.GetSubModelTitles(Id);
                    serializedList = JsonConvert.SerializeObject(List);
                    redisList = Encoding.UTF8.GetBytes(serializedList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    await distributedCache.SetAsync(cacheKey, redisList, options);
                }
                _logger.LogInformation($"SubModeController            GetTitles");
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
