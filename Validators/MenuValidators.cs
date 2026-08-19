using FluentValidation;
using LifestyleAPI.DTOs;

namespace LifestyleAPI.Validators
{
    public class CreateMenuDtoValidator : AbstractValidator<CreateMenuDTO>
    {
        public CreateMenuDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Menu name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Menu description is required.")
                .MaximumLength(250);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Menu price must be greater than 0.");
        }
    }

    public class UpdateMenuDtoValidator : AbstractValidator<UpdateMenuDTO>
    {
        public UpdateMenuDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Menu name cannot exceed 100 characters.")
                .When(x => x.Name is not null);

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Menu description cannot exceed 250 characters.")
                .When(x => x.Description is not null);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Menu price must be greater than 0.")
                .When(x => x.Price.HasValue);
        }
    }
}