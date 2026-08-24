using FluentValidation;
using LifestyleAPI.DTOs;

namespace LifestyleAPI.Validators
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDTO>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 10).WithMessage("Quantity must be between 1 and 10");

            RuleFor(x => x.SpecialInstructions)
                .MaximumLength(250).WithMessage("Maximum of 250 characters allowed");
        }
    }
}