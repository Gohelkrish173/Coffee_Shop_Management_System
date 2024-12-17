using FluentValidation;
using CoffeeShop_API.Models;

namespace DemoWebAPI.Validators
{
    public class CountryValidation : AbstractValidator<CountryModel>
    {
        public CountryValidation()
        {
            RuleFor(c => c.CountryName).NotNull().NotEmpty().WithMessage("Country Name Must Be Needed").IsInEnum().WithMessage("Please Enter Valid CountryName");
            RuleFor(c => c.CountryCode).NotNull().NotEmpty().WithMessage("Country Code Must Be needed");
        }
    }
}
