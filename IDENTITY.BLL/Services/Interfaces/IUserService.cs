using IDENTITY.BLL.DTO.Requests;
using IDENTITY.BLL.DTO.Responses;
using IDENTITY.DAL.Entities;

namespace IDENTITY.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> GetUserById(Guid Id);
        Task UpdateAsync(Guid Id, UserRequest client);
        Task DeleteAsync(Guid Id);
        Task ResetPassword(ResetPasswordRequest request);
        Task ForgotPassword(ForgotPasswordRequest request);
        Task<UserResponse> GetUserByEmail(string email);
        Task ForgotPasswordUnAuth(ForgotPasswordRequest request);
        Task SetPhoneNumber(Guid Id, string phonenumber);
        Task SendEmailConfirmation(Guid id, string email, string baseUrl);
        Task ConfirmEmail(ConfirmChangeEmailRequest request);
        Task<IEnumerable<UserVMResponse>> GetAllUsersAsync();

        Task<UserVMResponse> GetUserWithRole(Guid id);
        Task<IEnumerable<MechanicDTO>> GetMechanics();
        Task<MechanicDTO> GetMechanic(Guid Id);
    }
}
