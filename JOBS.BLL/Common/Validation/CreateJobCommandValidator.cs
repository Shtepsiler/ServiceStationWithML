using FluentValidation;
using JOBS.BLL.Operations.Jobs.Commands;

namespace JOBS.BLL.Common.Validation
{
    public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator()
        {
            RuleFor(request => request.VehicleId)
                .NotEmpty()
                .WithMessage("VehicleId can't be empty.")
                .NotNull()
                .WithMessage("VehicleId can't be Null.");


            /*         RuleFor(request => request.ManagerId)
                         .NotEmpty()
                         .WithMessage("Id can't be empty.")
                         .NotNull()
                         .WithMessage("Id can't be Null.");*/


            RuleFor(request => request.IssueDate)
                .NotEmpty()
                .WithMessage("IssueDate can't be empty.")
                .NotNull()
                .WithMessage("IssueDate can't be Null.");

            RuleFor(request => request.Description)
                .NotEmpty()
                .WithMessage("Description can't be empty.")
                .NotNull()
                .WithMessage("Description can't be Null.");

            RuleFor(request => request.ClientId)
                .NotEmpty()
                .WithMessage("ClientId can't be empty.")
                .NotNull()
                .WithMessage("ClientId can't be Null.");




        }
    }
}
