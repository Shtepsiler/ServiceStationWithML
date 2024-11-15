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
using System.Reflection.Metadata.Ecma335;


namespace ClientPartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    { 
        private readonly IBrandService brandService;
        private readonly ILogger<BrandController>? _logger;
        private readonly IDistributedCache? distributedCache;
        public BrandController(
             IBrandService brandService,
             ILogger<BrandController>? logger,
             IDistributedCache? distributedCache
             )
        {
            this.brandService = brandService;
            _logger = logger;
            this.distributedCache = distributedCache;
        }

      //  [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResponse>>> GetAllAsync()
        {
            try
            {

                var cacheKey = "BrandList";
                string serializedList;
                var List = new List<BrandResponse>();
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
                        List = JsonConvert.DeserializeObject<List<BrandResponse>>(serializedList);
                }
                else
                {
                    List = (List<BrandResponse>)await brandService.GetAllAsync();
                    try
                    {
                        serializedList = JsonConvert.SerializeObject(List);
                        redisList = Encoding.UTF8.GetBytes(serializedList);
                        var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                        await distributedCache.SetAsync(cacheKey, redisList, options);
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning($"Failed to set data to cache: {e.Message}");
                    }
                }


                _logger.LogInformation($"BrandController            GetAllAsync");
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
        public async Task<ActionResult<BrandResponse>> GetByIdAsync(Guid Id)
        {
            try
            {
                var result = await brandService.GetByIdAsync(Id);
                if (result == null)
                {
                    _logger.LogInformation($"Brand із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали Brand з бази даних!");
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
        public async Task<ActionResult> PostAsync([FromBody] BrandRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Brand є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Brand є некоректним");
                }
                var res = await brandService.PostAsync(brand);


                return Created("", res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі PostAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] BrandRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Brand є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Brand є некоректним");
                }

                await brandService.UpdateAsync(brand);
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
                var event_entity = await brandService.GetByIdAsync(id);
                if (event_entity == null)
                {
                    _logger.LogInformation($"Запис Brand із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await brandService.DeleteByIdAsync(id);
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
