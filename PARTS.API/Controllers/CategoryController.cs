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
    public class CategoryController : ControllerBase
    { 
        private readonly ILogger<CategoryController>? _logger;
        private readonly IDistributedCache? distributedCache;
        private readonly ICategoryService categoryService;
        public CategoryController(
             ICategoryService brandService,
             ILogger<CategoryController>? logger,
             IDistributedCache? distributedCache
             )
        {
            _logger = logger;
            this.distributedCache = distributedCache;
            this.categoryService = brandService;
        }

        //  [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAllAsync()
        {
            try
            {
                var cacheKey = "CategoryList";
                string serializedList;
                var List = new List<CategoryResponse>();
                byte[]? redisList = null;

                try
                {
                    redisList = await distributedCache.GetAsync(cacheKey);
       
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Failed to retrieve data from cache: {e.Message}");
                }

                if (redisList != null )
                {

                        serializedList = Encoding.UTF8.GetString(redisList);
                        List = JsonConvert.DeserializeObject<List<CategoryResponse>>(serializedList);
                  
                }
                else
                {
                    List = (List<CategoryResponse>)await categoryService.GetAllAsync();
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

                _logger.LogInformation($"CategoryController GetAllAsync");
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
        public async Task<ActionResult<CategoryResponse>> GetByIdAsync(Guid Id)
        {
            try
            {
                var result = await categoryService.GetByIdAsync(Id);
                if (result == null)
                {
                    _logger.LogInformation($"Categoty із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали Categoty з бази даних!");
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
        public async Task<ActionResult> PostAsync([FromBody] CategoryRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Categoty є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Categoty є некоректним");
                }
                var res = await categoryService.PostAsync(brand);


                return Created("", res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі PostAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

      
      //  [Authorize]
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] CategoryRequest brand)
        {
            try
            {
                if (brand == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Categoty є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Categoty є некоректним");
                }

                await categoryService.UpdateAsync(brand);
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
                var event_entity = await categoryService.GetByIdAsync(id);
                if (event_entity == null)
                {
                    _logger.LogInformation($"Запис Categoty із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await categoryService.DeleteByIdAsync(id);
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
