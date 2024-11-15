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
    public class OrderController : ControllerBase
    { 
        private readonly IOrderService orderService;
        private readonly ILogger<OrderController>? _logger;
        private readonly IDistributedCache? distributedCache;
        public OrderController(
             IOrderService orderService,
             ILogger<OrderController>? logger,
             IDistributedCache? distributedCache
             )
        {
            this.orderService = orderService;
            _logger = logger;
            this.distributedCache = distributedCache;
        }

      //  [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllAsync()
        {
            try
            {

                var cacheKey = "OrderList";
                string serializedList;
                var List = new List<OrderResponse>();
                byte[]? redisList = null;
                try
                {
                    redisList = await distributedCache.GetAsync(cacheKey);

                }
                catch (Exception e) { }
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    List = JsonConvert.DeserializeObject<List<OrderResponse>>(serializedList);
                        
                }
                else
                {
                    List = (List<OrderResponse>)await orderService.GetAllAsync(); 
                    serializedList = JsonConvert.SerializeObject(List);
                    redisList = Encoding.UTF8.GetBytes(serializedList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    await distributedCache.SetAsync(cacheKey, redisList, options);
                }
                _logger.LogInformation($"OrderController            GetAllAsync");
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
        public async Task<ActionResult<OrderResponse>> GetByIdAsync(Guid Id)
        {
            try
            {
                var result = await orderService.GetByIdAsync(Id);
                if (result == null)
                {
                    _logger.LogInformation($"Order із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали Order з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetNewOrder")]
        public async Task<ActionResult<OrderResponse>> GetNewOrderAsync()
        {
            try
            { 

                var neworder = new OrderRequest() {Id = Guid.NewGuid(),Timestamp = DateTime.Now};
                await orderService.PostAsync(neworder);
                if (neworder == null)
                {
                    _logger.LogInformation($"Order із Id: {neworder.Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали Order з бази даних!");
                    return Ok(neworder);
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
        public async Task<ActionResult> PostAsync([FromBody] OrderRequest Order)
        {
            try
            {
                if (Order == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Order є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Order є некоректним");
                }
                await orderService.PostAsync(Order);


                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі PostAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("AddPartToOrder")]
        public async Task<ActionResult> AddPartToOrderAsync([FromQuery] Guid orderId, [FromQuery] Guid partId)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Order є некоректним");
                }
                await orderService.AddPartToOrderAsync(orderId,  partId);


                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі PostAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("RemovePartFromOrder")]
        public async Task<ActionResult> RemovePartFromOrderAsync([FromQuery] Guid orderId, [FromQuery] Guid partId)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Order є некоректним");
                }
                await orderService.RemovePartFromOrderAsync(orderId, partId);


                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі PostAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //  [Authorize]
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] OrderRequest Order)
        {
            try
            {
                if (Order == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Order є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Order є некоректним");
                }

                await orderService.UpdateAsync(Order);
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
                var event_entity = await orderService.GetByIdAsync(id);
                if (event_entity == null)
                {
                    _logger.LogInformation($"Запис Order із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await orderService.DeleteByIdAsync(id);
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
