using AutoMapper;
using AutoMapper.Configuration.Annotations;
using IDENTITY.BLL.Configurations;
using IDENTITY.BLL.DTO.Requests;
using IDENTITY.BLL.DTO.Responses;
using IDENTITY.BLL.Services.Interfaces;
using IDENTITY.DAL.Data;
using IDENTITY.DAL.Entities;
using IDENTITY.DAL.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace IDENTITY.BLL.Services
{
    public class IdentityService : IIdentityService
    {

        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager; 
        private readonly AppDBContext dbContext;
        private readonly ITokenService tokenService;
        private readonly EmailSender emailSender;
        private readonly ClientAppConfiguration client;
        private readonly ILogger<IdentityService> logger;

        public IdentityService(
        IMapper mapper,
        ITokenService tokenService,
        UserManager<User> userManager,
        AppDBContext dbContext
,
        EmailSender emailSender,
        ClientAppConfiguration client,
        ILogger<IdentityService> logger,
        SignInManager<User> signInManager)
        {
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.emailSender = emailSender;
            this.client = client;
            this.logger = logger;
            this.signInManager = signInManager;
        }
        public async Task ConfirmEmail(ConfirmEmailRequest request)
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString())
                  ?? throw new EntityNotFoundException(
                      $"{nameof(User)} with user ID {request.Id} not found.");

            var rez = await userManager.ConfirmEmailAsync(user, request.Code);

            if (rez.Succeeded)
            {
                user.EmailConfirmed = true;
                await dbContext.SaveChangesAsync();
                logger.Log(LogLevel.Information, $"                                                                        User id {request.Id} confirm email");


            }
            else
            {
                logger.Log(LogLevel.Information, $"                                                                        User id {request.Id} not confirm email");

                throw new EmailNotConfirmedException("Email is not confirmed");
            }

        }

        public async Task<JwtResponse> SignInAsync(UserSignInRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email)
                ?? throw new EntityNotFoundException( $"Incorrect username or password.");


            var sres = await signInManager.PasswordSignInAsync(user,request.Password,request.RememberMe,false);

            if (!sres.Succeeded)
            {
                logger.Log(LogLevel.Information, $"                                                                        User {request.Email} Sign in failure");

                throw new EntityNotFoundException("Incorrect username or password.");

            }
            if (!user.EmailConfirmed)
            {
               await SendEmailConfirmation(user.Id,request.refererUrl);
            }
            logger.Log(LogLevel.Information, $"                                                                        User {request.Email} is Sign in successfully");

            var jwtToken = tokenService.BuildToken(user);
            return new() { Id = user.Id, Token = tokenService.SerializeToken(jwtToken), UserName = user.UserName, IsEmailConfirmed = user.EmailConfirmed, RequiresTwoFactor = user.TwoFactorEnabled };
        }

        public async Task SendEmailConfirmation(Guid userid,string refererUrl)
        {
            var user = await userManager.FindByIdAsync(userid.ToString());
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = $"{refererUrl}{client.EmailConfirmationPath}?Id={userid}&Code={System.Net.WebUtility.UrlEncode(code)}";

            await emailSender.SendEmailAsync(user.Email, "Confirm your email", $"{client.ResetPasswordMessage} {callbackUrl}");
            throw new EmailNotConfirmedException("email not confirmed");
        }



        public async Task SignOutAsync(Guid id)
        {
          await signInManager.SignInAsync(await userManager.FindByIdAsync(id.ToString()),false);
        }

        public async Task<JwtResponse> SignUpAsync(UserSignUpRequest request)
        {
            var user = mapper.Map<UserSignUpRequest, User>(request);
            var signUpResult = await userManager.CreateAsync(user, request.Password);

            if (!signUpResult.Succeeded)
            {
                string errors = string.Join("\n",
                    signUpResult.Errors.Select(error => error.Description));

                throw new ArgumentException(errors);
            }
            await userManager.AddToRoleAsync(user, "User");
            await dbContext.SaveChangesAsync();
            var newUser = await userManager.FindByNameAsync(request.UserName);


            if (!user.EmailConfirmed)
            {
                await SendEmailConfirmation(user.Id, request.refererUrl);

            }

            logger.Log(LogLevel.Information, $"                                                                        User {request.UserName} is Sign up successfully");


            try
            {
                //  var newUser = await userManager.FindByNameAsync(request.UserName);
                var jwtToken = tokenService.BuildToken(user);
                return new() { Id = newUser.Id, UserName = newUser.UserName, Token = tokenService.SerializeToken(jwtToken), IsEmailConfirmed = user.EmailConfirmed, RequiresTwoFactor = user.TwoFactorEnabled };
            }
            catch (Exception ex) { throw ex; }
        }


    }
}
