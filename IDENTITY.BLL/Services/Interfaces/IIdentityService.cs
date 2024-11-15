using IDENTITY.BLL.DTO.Requests;
using IDENTITY.BLL.DTO.Responses;

namespace IDENTITY.BLL.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<JwtResponse> SignInAsync(UserSignInRequest request);

        Task<JwtResponse> SignUpAsync(UserSignUpRequest request);
        Task SignOutAsync(Guid id);
        Task ConfirmEmail(ConfirmEmailRequest request);
        Task SendEmailConfirmation(Guid userid, string refererUrl);
    }
}
