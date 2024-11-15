using IDENTITY.API.Attributes;
using IDENTITY.BLL.DTO.Requests;
using IDENTITY.BLL.DTO.Responses;
using IDENTITY.BLL.Services.Interfaces;
using IDENTITY.DAL.Entities;
using IDENTITY.DAL.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IDENTITY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> userManager;
        public UserController(
            ILogger<UserController> logger,
             IUserService userService,
             UserManager<User> userManager)
        {
            _logger = logger;
            this.userService = userService;
            this.userManager = userManager;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("AllUsers")]
        public async Task<ActionResult<UserVMResponse>> GetAllAsync()
        {
            try
            {
                var result = await userService.GetAllUsersAsync();

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали юзера з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechanic,User")]
        [HttpGet("GetMechanics")]
        public async Task<ActionResult<IEnumerable<MechanicDTO>>> GetMechanics()
        {
            try
            {
                var result = await userService.GetMechanics();

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали механіків з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetMechanics() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Mechanic,User")]
        [HttpGet("GetMechanic")]
        public async Task<ActionResult<MechanicDTO>> GetMechanic(Guid Id)
        {
            try
            {
                var result = await userService.GetMechanic(Id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали механіків з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetMechanics() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [CustomAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,User,Mechanic")]
        [HttpPut("SetPhoneNumber/{Id}")]
        public async Task<ActionResult> SetPhoneNumber(Guid Id, [FromBody] string PhoneNumber)
        {
            SetPhoneNumberRequest request = new() { Id = Id, PhoneNumber = PhoneNumber };
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault().ErrorMessage));
            try
            {
                await userService.SetPhoneNumber(request.Id, request.PhoneNumber);
                return Ok();
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }

        }
        [CustomAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,User,Mechanic")]
        [HttpGet("{Id}")]
        public async Task<ActionResult<UserResponse>> GetByIdAsync(Guid Id)
        {
            try
            {
                var result = await userService.GetUserById(Id);

                if (result == null)
                {
                    _logger.LogInformation($"Юзер із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали юзера з бази даних!");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [CustomAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("GetWithRole/{Id}")]
        public async Task<ActionResult<UserResponse>> GetByIdWithRoleAsync(Guid Id)
        {
            try
            {
                var result = await userService.GetUserWithRole(Id);
                if (result == null)
                {
                    _logger.LogInformation($"Юзер із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали юзера з бази даних!");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("GetByEmail")]
        public async Task<ActionResult<UserResponse>> GetByEmailAsync([FromQuery] string Email)
        {
            try
            {
                var result = await userService.GetUserByEmail(Email);

                if (result == null)
                {
                    _logger.LogInformation($"Юзер із Email: {Email}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали івент з бази даних!");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetByNameAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [CustomAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,User,Mechanic")]
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateAsync( [FromBody] UserRequest client)
        {
            try
            {
                if (client == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт  є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт  є некоректним");
                }
                await userService.UpdateAsync(client.Id, client);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі UpdateAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [CustomAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,User,Mechanic")]
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteByIdAsync(Guid Id)
        {
            try
            {
                var client = await userService.GetUserById(Id);
                if (client == null)
                {
                    _logger.LogInformation($"Запис із Id: {Id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                await userService.DeleteAsync(client.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteByNameAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] Guid Id, [FromQuery] string Code, [FromBody] string newpas)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault().ErrorMessage));
            var request = new ResetPasswordRequest() { Id = Id, Code = Code, NewPasword = newpas };
            try
            {
                await userService.ResetPassword(request);
                return Ok();
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string Email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault().ErrorMessage));
            var request = new ForgotPasswordRequest() { Email = Email };
            var refererUrl = HttpContext.Request.Headers.Referer.ToString();
            var uri = new Uri(refererUrl);
            var baseUrl = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
            request.referer = baseUrl;
            try
            {
                await userService.ForgotPassword(request);
                return Ok();
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }
        }
        [HttpPost("ForgotPasswordUnAuth")]
        public async Task<IActionResult> ForgotPasswordFromUnAuth([FromQuery] string Email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault().ErrorMessage));
            var request = new ForgotPasswordRequest() { Email = Email };
            var refererUrl = HttpContext.Request.Headers.Referer.ToString();
            var uri = new Uri(refererUrl);
            var baseUrl = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
            request.referer = baseUrl;
            try
            {
                await userService.ForgotPasswordUnAuth(request);
                return Ok();
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }
        }
        [HttpPut("ChangeEmail")]
        [CustomAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,User,Mechanic")]
        public async Task<IActionResult> ChangeEmail([FromQuery] Guid Id, [FromBody] string Email)
        {
            try
            {
                var refererUrl = HttpContext.Request.Headers["Referer"].ToString();
                var uri = new Uri(refererUrl);
                var baseUrl = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
                var user = await userManager.FindByEmailAsync(Email);
                await userService.SendEmailConfirmation(user.Id, Email, baseUrl);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("ConfirmChangeEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmChangeEmailRequest request)
        {
            try
            {
                if (request == null) { throw new ArgumentNullException(nameof(request)); }

                await userService.ConfirmEmail(request);
                return Ok();
            }
            catch (EmailNotConfirmedException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }
        }
    }
}

