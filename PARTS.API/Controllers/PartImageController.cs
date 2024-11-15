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
    public class PartImageController : ControllerBase
    { 
        private readonly ILogger<PartImageController> _logger;
        private readonly IDistributedCache distributedCache;
        private readonly IPartImageService partImageService;
        public PartImageController(
            ILogger<PartImageController> logger,
             IDistributedCache distributedCache
,
             IPartImageService PartImageService
             )
        {
            _logger = logger;
            this.distributedCache = distributedCache;
            this.partImageService = PartImageService;
        }

      //  [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartImageResponse>>> GetAllAsync()
        {
            try
            {

                var cacheKey = "PartImageList";
                string serializedList;
                var List = new List<PartImageResponse>();
                var redisList = await distributedCache.GetAsync(cacheKey);
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    List = JsonConvert.DeserializeObject<List<PartImageResponse>>(serializedList);
                }
                else
                {
                    List = (List<PartImageResponse>)await partImageService.GetAllAsync(); 
                    serializedList = JsonConvert.SerializeObject(List);
                    redisList = Encoding.UTF8.GetBytes(serializedList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    await distributedCache.SetAsync(cacheKey, redisList, options);
                }
                _logger.LogInformation($"PartImageController            GetAllAsync");
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
        public async Task<ActionResult<PartImageResponse>> GetByIdAsync(Guid Id)
        {
            try
            {
                var result = await partImageService.GetByIdAsync(Id);
                if (result == null)
                {
                    _logger.LogInformation($"PartImage із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали PartImage з бази даних!");
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
        public async Task<ActionResult> PostAsync([FromBody] PartImageRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт PartImage є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт PartImage є некоректним");
                }
                await partImageService.PostAsync(brand);


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
        public async Task<ActionResult> UpdateAsync([FromBody] PartImageRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт PartImage є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт PartImage є некоректним");
                }

                await partImageService.UpdateAsync(brand);
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
                var event_entity = await partImageService.GetByIdAsync(id);
                if (event_entity == null)
                {
                    _logger.LogInformation($"Запис PartImage із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await partImageService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
