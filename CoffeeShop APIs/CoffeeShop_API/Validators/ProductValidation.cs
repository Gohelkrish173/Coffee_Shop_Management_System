using CoffeeShop_API.Models;
using FluentValidation;

namespace CoffeeShop_API.Validators
{
    public class ProductValidation : AbstractValidator<ProductModel>
    {
       public ProductValidation() 
        {
            RuleFor(P => P.ProductName).NotNull().NotEmpty().WithMessage("ProductName Must Be Needed").IsInEnum().WithMessage("Enter Valid ProductName");
            RuleFor(P => P.ProductPrice).NotNull().NotEmpty().WithMessage("ProductPrice Must Be Required.");
            RuleFor(P => P.UserID).NotNull().NotEmpty().WithMessage("UserID Must Be Required.");
            RuleFor(P => P.ProductCode).NotNull().NotEmpty().WithMessage("ProductCode is must be required.").MaximumLength(3).WithMessage("required only 3 character");
            RuleFor(P => P.Description).NotNull();
        }
    }
}
