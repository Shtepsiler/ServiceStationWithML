using AutoMapper;
using IDENTITY.BLL.Configurations;
using IDENTITY.BLL.DTO.Requests;
using IDENTITY.BLL.DTO.Responses;
using IDENTITY.BLL.Services.Interfaces;
using IDENTITY.DAL.Data;
using IDENTITY.DAL.Entities;
using IDENTITY.DAL.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IDENTITY.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;
        private readonly ClientAppConfiguration client;
        private readonly AppDBContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly EmailSender emailSender;
        private readonly RoleManager<Role> roleManager;

        public UserService(IMapper mapper, ITokenService tokenService, AppDBContext dbContext, UserManager<User> userManager, EmailSender emailSender, IConfiguration configuration, ClientAppConfiguration clientAppConfiguration, RoleManager<Role> roleManager)
        {
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.configuration = configuration;
            client = clientAppConfiguration;
            this.roleManager = roleManager;
        }
        public async Task ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                var user = await userManager.FindByIdAsync(request.Id.ToString());
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    throw new EntityNotFoundException("User not found");
                }
                if (!(await userManager.IsEmailConfirmedAsync(user)))
                {
                    throw new EmailNotConfirmedException("Email not confirmed");
                }
                var result = await userManager.ResetPasswordAsync(user, request.Code, request.NewPasword);

                if (!result.Succeeded)
                {
                    throw new ArgumentException(string.Join("\n",
                        result.Errors.Select(error => error.Description)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task ForgotPasswordUnAuth(ForgotPasswordRequest request)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
                if (user == null)
                    throw new EntityNotFoundException("User not found");
                if (!(await userManager.IsEmailConfirmedAsync(user)))
                {
                    throw new EmailNotConfirmedException("Email not confirmed");
                }
                var code = await userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);

                var callbackUrl = $"{request.referer}{client.ResetPasswordUnAuthPath}?Id={user.Id}&Code={code}";
                await emailSender.SendEmailAsync(request.Email, "Reset password", callbackUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task ForgotPassword(ForgotPasswordRequest request)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
                if (user == null)
                    throw new EntityNotFoundException("User not found");
                if (!(await userManager.IsEmailConfirmedAsync(user)))
                {
                    throw new EmailNotConfirmedException("Email not confirmed");
                }
                var code = await userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);

                var callbackUrl = $"{request.referer}{client.ResetPasswordPath}?Id={user.Id}&Code={code}";
                await emailSender.SendEmailAsync(request.Email, "Reset password", callbackUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<UserVMResponse>> GetAllUsersAsync()
        {
            try
            {
                var usersWithRoles = await dbContext.Users
                    .Join(
                        dbContext.UserRoles,
                        user => user.Id,
                        userRole => userRole.UserId,
                        (user, userRole) => new { User = user, UserRole = userRole }
                    )
                    .Join(
                        dbContext.Roles,
                        userUserRole => userUserRole.UserRole.RoleId,
                        role => role.Id,
                        (userUserRole, role) => new { User = userUserRole.User, Role = role }
                    )
                    .Select(ur => new
                    {
                        ur.User.Id,
                        ur.User.UserName,
                        ur.User.Email,
                        ur.User.PhoneNumber,
                        ur.User.EmailConfirmed,
                        ur.User.TwoFactorEnabled,
                        ur.Role.Name
                    })
                    .ToListAsync();

                var usersVM = usersWithRoles.Select(u =>
                {
                    var userVM = new UserVMResponse()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        PhoneNumber = u.PhoneNumber,
                        UserName = u.UserName,
                    };
                    userVM.Role = u.Name; 
                    return userVM;
                });

                return usersVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserVMResponse> GetUserWithRole(Guid id)
        {
            try
            {
                var usersWithRoles = await dbContext.Users
                    .Join(
                        dbContext.UserRoles,
                        user => user.Id,
                        userRole => userRole.UserId,
                        (user, userRole) => new { User = user, UserRole = userRole }
                    )
                    .Join(
                        dbContext.Roles,
                        userUserRole => userUserRole.UserRole.RoleId,
                        role => role.Id,
                        (userUserRole, role) => new { User = userUserRole.User, Role = role }
                    )
                    .Select(ur => new
                    {
                        ur.User.Id,
                        ur.User.UserName,
                        ur.User.Email,
                        ur.User.PhoneNumber,
                        ur.User.EmailConfirmed,
                        ur.User.TwoFactorEnabled,
                        ur.Role.Name
                    })
                    .ToListAsync();

                var usersVM = usersWithRoles.Select(u =>
                {
                    var userVM = new UserVMResponse()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        PhoneNumber = u.PhoneNumber,
                        UserName = u.UserName,
                    };
                    userVM.Role = u.Name; 
                    return userVM;
                }).Where(p => p.Id == id).FirstOrDefault();

                return usersVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserResponse> GetUserById(Guid Id)
        {
            try
            {
                string id = Id.ToString();
                var user = await userManager.FindByIdAsync(id);
                if (user == null) throw new EntityNotFoundException("User not found");
                var userDTO = mapper.Map<UserResponse>(user);
                userDTO.hasPassword = await userManager.HasPasswordAsync(user);
                return userDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserResponse> GetUserByEmail(string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null) throw new EntityNotFoundException("User not found");
                var userDTO = mapper.Map<UserResponse>(user);
                userDTO.hasPassword = await userManager.HasPasswordAsync(user);
                return userDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateAsync(Guid Id, UserRequest client)
        {
            try
            {
                var user = await userManager.FindByIdAsync(Id.ToString());
                if (user == null) throw new EntityNotFoundException("User not found");
                user.UserName = client.UserName;
                user.PhoneNumber = client.PhoneNumber;
                user.Email = client.Email;
                await userManager.UpdateAsync(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteAsync(Guid Id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(Id.ToString());
                if (user == null) throw new EntityNotFoundException("User not found");
                await userManager.DeleteAsync(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task SetPhoneNumber(Guid Id, string phonenumber)
        {
            try
            {
                var user = await userManager.FindByIdAsync(Id.ToString());
                if (user == null) throw new EntityNotFoundException("User not found");
                await userManager.SetPhoneNumberAsync(user, phonenumber);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task SendEmailConfirmation(Guid id, string email, string baseUrl)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id.ToString())
                      ?? throw new EntityNotFoundException(
                          $"{nameof(User)} with user ID {id} not found.");
                await SendChangeEmailConfirmation(id, baseUrl, email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task SendChangeEmailConfirmation(Guid userid, string refererUrl, string newemail)
        {
            var user = await userManager.FindByIdAsync(userid.ToString());
            var code = await userManager.GenerateChangeEmailTokenAsync(user, newemail);
            var callbackUrl = $"{refererUrl}{client.EmailConfirmationPath}?Id={userid}&Email={newemail}&Code={System.Net.WebUtility.UrlEncode(code)}";
            await emailSender.SendEmailAsync(user.Email, "Confirm your email", $"{client.ResetPasswordMessage} {callbackUrl}");
            throw new EmailNotConfirmedException("email not confirmed");
        }
        public async Task ConfirmEmail(ConfirmChangeEmailRequest request)
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString())
                  ?? throw new EntityNotFoundException(
                      $"{nameof(User)} with user ID {request.Id} not found.");
            var rez = await userManager.ChangeEmailAsync(user, request.Email, request.Code);
            if (rez.Succeeded)
            {
                user.EmailConfirmed = true;
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new EmailNotConfirmedException("Email is not confirmed");
            }
        }
        public async Task<IEnumerable<MechanicDTO>> GetMechanics()
        {
            try
            {
                var usersWithRoles = await dbContext.Users
                    .Join(
                        dbContext.UserRoles,
                        user => user.Id,
                        userRole => userRole.UserId,
                        (user, userRole) => new { User = user, UserRole = userRole }
                    )
                    .Join(
                        dbContext.Roles,
                        userUserRole => userUserRole.UserRole.RoleId,
                        role => role.Id,
                        (userUserRole, role) => new { User = userUserRole.User, Role = role }
                    )
                    .Select(ur => new
                    {
                        ur.User.Id,
                        ur.User.UserName,
                        ur.User.Email,
                        ur.User.PhoneNumber,
                        ur.User.EmailConfirmed,
                        ur.User.TwoFactorEnabled,
                        ur.Role.Name
                    })
                    .Where(p => p.Name.Equals("Mechanic"))
                    .ToListAsync();
                var usersVM = usersWithRoles.Select(u =>
                {
                    var userVM = new MechanicDTO()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        UserName = u.UserName,
                    };
                    return userVM;
                });

                return usersVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MechanicDTO> GetMechanic(Guid Id)
        {
            try
            {
                var usersWithRoles = await dbContext.Users
                    .Join(
                        dbContext.UserRoles,
                        user => user.Id,
                        userRole => userRole.UserId,
                        (user, userRole) => new { User = user, UserRole = userRole }
                    )
                    .Join(
                        dbContext.Roles,
                        userUserRole => userUserRole.UserRole.RoleId,
                        role => role.Id,
                        (userUserRole, role) => new { User = userUserRole.User, Role = role }
                    )
                    .Select(ur => new
                    {
                        ur.User.Id,
                        ur.User.UserName,
                        ur.User.Email,
                        ur.User.PhoneNumber,
                        ur.User.EmailConfirmed,
                        ur.User.TwoFactorEnabled,
                        ur.Role.Name
                    })
                    .Where(p => p.Name.Equals("Mechanic"))
                    .ToListAsync();

                var usersVM = usersWithRoles.Select(u =>
                {
                    var userVM = new MechanicDTO()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        UserName = u.UserName,
                    };
                    return userVM;
                });
                var mechanic = usersVM.Where(p => p.Id == Id).First();
                return mechanic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
