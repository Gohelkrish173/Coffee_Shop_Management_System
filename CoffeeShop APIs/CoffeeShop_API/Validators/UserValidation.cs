using CoffeeShop_API.Models;
using FluentValidation;

namespace CoffeeShop_API.Validators
{
    public class UserValidation : AbstractValidator<UserModel>
    {
        public UserValidation() 
        {
            RuleFor(u => u.UserName).NotNull().NotEmpty().WithMessage("UserName is Must Be Required.").IsInEnum().WithMessage("Enter Valid UserName.");
            RuleFor(u => u.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotNull().NotEmpty().WithMessage("Password Must be Contain 8 character.");
            RuleFor(u => u.MobileNo).NotNull().NotEmpty().WithMessage("Mobile Number must be taken 10 digits");
            RuleFor(u => u.Address).NotNull().NotEmpty().WithMessage("Address is Must be required.");
            RuleFor(u => u.IsActive).NotNull();
        }
    }
}
