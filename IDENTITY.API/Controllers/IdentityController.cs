using FluentValidation;
using Google.Apis.Auth;
using IDENTITY.BLL.Configurations;
using IDENTITY.BLL.DTO.Requests;
using IDENTITY.BLL.DTO.Responses;
using IDENTITY.BLL.Services.Interfaces;
using IDENTITY.DAL.Entities;
using IDENTITY.DAL.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IDENTITY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IValidator<UserSignUpRequest> _SingUpValidator;
        private readonly IValidator<UserSignInRequest> _SingInValidator;
        private readonly IIdentityService _IdentityService;
        private readonly GoogleClientConfiguration googleClientConfiguration;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<IdentityController> logger;
        private readonly ITokenService tokenService;

        public IdentityController(IValidator<UserSignInRequest> singinvalidator, IValidator<UserSignUpRequest> singupvalidator, IIdentityService identityService, GoogleClientConfiguration googleClientConfiguration, UserManager<User> userService, ITokenService tokenService, ILogger<IdentityController> logger, SignInManager<User> signInManager)
        {
            _SingInValidator = singinvalidator;
            _SingUpValidator = singupvalidator;
            _IdentityService = identityService;
            this.googleClientConfiguration = googleClientConfiguration;
            userManager = userService;
            this.tokenService = tokenService;
            this.logger = logger;
            this.signInManager = signInManager;
        }

        [HttpPost("signUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JwtResponse>> SignUpAsync(
       [FromBody] UserSignUpRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Request cannot be null");
                }
                var refererUrl = HttpContext.Request.Headers["Referer"].ToString();
                if (string.IsNullOrEmpty(refererUrl))
                {
                    throw new ArgumentException("Referer URL cannot be null or empty");
                }
                var uri = new Uri(refererUrl);
                var baseUrl = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
                request.refererUrl = baseUrl;
                var validationResult = _SingUpValidator.Validate(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("Validation failed for SignIn request: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }
                var response = await _IdentityService.SignUpAsync(request);
                HttpContext.Response.Cookies.Append("Bearer", response.Token, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(2),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });
                return Ok(response);
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogError(ex, "Invalid URI format during SignIn.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (EmailNotConfirmedException ex)
            {
                logger.LogError(ex, "Email not confirmed.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validation exception during SignIn.");
                return BadRequest(new { Error = ex.Errors });
            }
            catch (ArgumentNullException ex)
            {
                logger.LogError(ex, "Argument null exception during SignIn.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (UriFormatException ex)
            {
                logger.LogError(ex, "Invalid URI format during SignIn.");
                return BadRequest(new { Error = "Invalid referer URL format" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during SignIn.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPost("signIn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JwtResponse>> SignInAsync(
            [FromBody] UserSignInRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogWarning("SignIn request is null.");
                    return BadRequest("Request cannot be null.");
                }
                var refererUrl = HttpContext.Request.Headers.Referer.ToString();
                if (string.IsNullOrEmpty(refererUrl))
                {
                    logger.LogWarning("Referer URL is missing.");
                    return BadRequest("Referer URL is required.");
                }
                var uri = new Uri(refererUrl);
                var baseUrl = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
                request.refererUrl = baseUrl;
                var validationResult = _SingInValidator.Validate(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("Validation failed for SignIn request: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }
                var response = await _IdentityService.SignInAsync(request);
                if (response == null)
                {
                    logger.LogWarning("SignIn failed for user: {Username}", request.Email);
                    return NotFound("Invalid credentials.");
                }
                HttpContext.Response.Cookies.Append("Bearer", response.Token, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });
                logger.LogInformation("User {Username} signed in successfully.", request.Email);
                return Ok(response);
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogError(ex, "Invalid URI format during SignIn.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (EmailNotConfirmedException ex)
            {
                logger.LogError(ex, "Email not confirmed.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validation exception during SignIn.");
                return BadRequest(new { Error = "Validation failed", Details = ex.Errors });
            }
            catch (ArgumentNullException ex)
            {
                logger.LogError(ex, "Argument null exception during SignIn.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (UriFormatException ex)
            {
                logger.LogError(ex, "Invalid URI format during SignIn.");
                return BadRequest(new { Error = "Invalid referer URL format" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during SignIn.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "An unexpected error occurred", Details = ex.Message });
            }
        }

        [HttpPost("ConfirmEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
            try
            {
                if (request == null) { throw new ArgumentNullException(nameof(request)); }


                await _IdentityService.ConfirmEmail(request);
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

        [HttpPost("RefreshSignIn")]
        public async Task<IActionResult> RefreshSignInAsync(Guid id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null) return Unauthorized();

                await signInManager.RefreshSignInAsync(user);
                var jwtToken = tokenService.BuildToken(user);
                HttpContext.Response.Cookies.Append("Bearer", tokenService.SerializeToken(jwtToken), new()
                {
                    Expires = DateTime.Now.AddDays(2),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ResendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmailAsync([FromBody] string Email)
        {
            try
            {
                var refererUrl = HttpContext.Request.Headers["Referer"].ToString();
                var uri = new Uri(refererUrl);
                var baseUrl = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
                var user = await userManager.FindByEmailAsync(Email);
                await _IdentityService.SendEmailConfirmation(user.Id, baseUrl);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
