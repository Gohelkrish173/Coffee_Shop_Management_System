using CoffeeShop_API.Models;
using FluentValidation;

namespace CoffeeShop_API.Validators
{
    public class CustomerValidation : AbstractValidator<CustomerModel>
    {
        public CustomerValidation() 
        {
            RuleFor(C => C.CustomerName).NotNull().NotEmpty().WithMessage("CustomerName Must Be Needed").IsInEnum().WithMessage("Enter Valid CustomerName");
            RuleFor(C => C.HomeAddress).NotNull().NotEmpty().WithMessage("HomeAddress Must Be Needed");
            RuleFor(C => C.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(C => C.MobileNO).NotNull().NotEmpty().WithMessage("MobileNumber Can't be Empy").MaximumLength(10).WithMessage("Please Enter Valid Number");
            RuleFor(C => C.GST_NO).NotNull().NotEmpty().WithMessage("GSTNumber Can't be Empy").MaximumLength(12).WithMessage("Please Enter Valid GST Number");
            RuleFor(C => C.CityName).NotNull().NotEmpty().WithMessage("City Name Must Be Required.").IsInEnum().WithMessage("Enter Valid CityName");
            RuleFor(C => C.PinCode).NotNull().NotEmpty().WithMessage("PINCode Must Be Required.");
            RuleFor(C => C.NetAmount).NotNull().NotEmpty().WithMessage("Net Amount Must Be Required.");
            RuleFor(C => C.UserID).NotNull().NotEmpty().WithMessage("UserID Must Be Required.");
        }
    }
}
