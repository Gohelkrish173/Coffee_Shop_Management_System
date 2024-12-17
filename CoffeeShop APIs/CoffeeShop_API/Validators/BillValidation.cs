using CoffeeShop_API.Models;
using FluentValidation;

namespace CoffeeShop_API.Validators
{
    public class BillValidation : AbstractValidator<BillModel>
    {
        public BillValidation() 
        {
            RuleFor(B => B.BillNumber).NotNull().NotEmpty().WithMessage("BillNumber Must Be Needed");
            RuleFor(B => B.BillDate).NotNull().NotEmpty().WithMessage("BillDate Must Be Needed");
            RuleFor(B => B.OrderID).NotNull().NotEmpty().WithMessage("OrderID Must Be Required.");
            RuleFor(B => B.Discount).NotNull();
            RuleFor(B => B.TotalAmount).NotNull().WithMessage("Total Amount Must Be Required.");
            RuleFor(B => B.NetAmount).NotNull().NotEmpty().WithMessage("Net Amount Must Be Required.");
            RuleFor(B => B.UserID).NotNull().NotEmpty().WithMessage("UserID Must Be Required.");
        }
    }
}
