using FluentValidation;
using JOBS.BLL.Operations.Jobs.Commands;


namespace JOBS.BLL.Common.Validation
{
    public class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
    {
        public UpdateJobCommandValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty()
                .WithMessage("Id can't be empty.")
                .NotNull()
                .WithMessage("Id can't be Null.");

            RuleFor(request => request.ClientId)
                .NotEmpty()
                .WithMessage("ClientId can't be empty.")
                .NotNull()
                .WithMessage("Id can't be Null.");


            RuleFor(request => request.ModelId)
                .NotEmpty()
                .WithMessage("ModelId can't be empty.")
                .NotNull()
                .WithMessage("Id can't be Null.");


            RuleFor(request => request.IssueDate)
                .NotEmpty()
                .WithMessage("IssueDate can't be empty.")
                .NotNull()
                .WithMessage("Id can't be Null.");

            RuleFor(request => request.Description)
                .NotEmpty()
                .WithMessage("Description can't be empty.")
                .NotNull()
                .WithMessage("Id can't be Null.");




        }
    }
}
