using CoffeeShop_API.Models;
using FluentValidation;

namespace CoffeeShop_API.Validators
{
    public class OrderDetailValidation : AbstractValidator<OrderDetailModel>
    {
        public OrderDetailValidation() 
        {
            RuleFor(OD => OD.OrderID).NotNull().NotEmpty().WithMessage("OrderID Must Be Required.");
            RuleFor(OD => OD.ProductID).NotNull().NotEmpty().WithMessage("ProductID Must Be Required.");
            RuleFor(OD => OD.TotalAmount).NotNull().NotEmpty().WithMessage("Total Amount Must Be Required.");
            RuleFor(OD => OD.Amount).NotNull().NotEmpty().WithMessage("Amount Must Be Required.");
            RuleFor(OD => OD.Quantity).NotNull().IsInEnum().WithMessage("Please Enter Int Value");
            RuleFor(OD => OD.UserID).NotNull().NotEmpty().WithMessage("UserID Must Be Required.");
        }
    }
}
