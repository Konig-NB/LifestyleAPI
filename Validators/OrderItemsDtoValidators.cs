using FluentValidation;
using LifestyleAPI.DTOs;

namespace LifestyleAPI.Validators
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDTO>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.MenuItemId)
                .GreaterThan(0).WithMessage("MenuItemId must be a valid menu item.");

            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 10).WithMessage("Quantity must be between 1 and 10");

            RuleFor(x => x.SpecialInstructions)
                .MaximumLength(250).WithMessage("Maximum of 250 characters allowed");
        }
    }

    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDTO>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("An order must contain at least one item.");

            RuleForEach(x => x.OrderItems)
                .SetValidator(new CreateOrderItemDtoValidator());
        }
    }
}