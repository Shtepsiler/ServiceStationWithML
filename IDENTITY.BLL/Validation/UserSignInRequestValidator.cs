using FluentValidation;
using IDENTITY.BLL.DTO.Requests;

namespace IDENTITY.BLL.Validation
{
    public class UserSignInRequestValidator : AbstractValidator<UserSignInRequest>
    {
        public UserSignInRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty()
                .WithMessage("UserName can't be empty.");

            RuleFor(request => request.Password)
                .NotEmpty()
                .WithMessage("Password can't be empty.")
                .MinimumLength(8)
                .WithMessage(request => $"{nameof(request.Password)} must be longer then 8 character");
        }
    }
}
