using CoffeeShop_API.Models;
using FluentValidation;

namespace CoffeeShop_API.Validators
{
    public class OrderValidation : AbstractValidator<OrderModel>
    {
        public OrderValidation() 
        {
            RuleFor(O => O.OrderNO).NotNull().NotEmpty().WithMessage("OrderNO Must Be Needed");
            RuleFor(O => O.OrderDate).NotNull().NotEmpty().WithMessage("OrderDate Must Be Needed");
            RuleFor(O => O.CustomerID).NotNull().NotEmpty().WithMessage("CustomerID Must Be Required.");
            RuleFor(O => O.PaymentMode).NotNull().NotEmpty().WithMessage("Payment Must Be Needed");
            RuleFor(O => O.UserID).NotNull().NotEmpty().WithMessage("UserID Must Be Required.");
            RuleFor(O => O.TotalAmount).NotNull().NotEmpty().WithMessage("Total Amount Must Be Required.");
            RuleFor(O => O.ShippingAddress).NotNull().NotEmpty().WithMessage("ShippedAddress Must Be Needed.");
        }
    }
}
