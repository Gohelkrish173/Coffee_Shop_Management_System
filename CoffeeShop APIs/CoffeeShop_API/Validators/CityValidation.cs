using CoffeeShop_API.Models;
using FluentValidation;

namespace DemoWebAPI.Validators
{
    public class CityValidation : AbstractValidator<CityModel>
    {
        public CityValidation()
        {
            RuleFor(c => c.CityName).NotNull().NotEmpty().WithMessage("CityName is must be required.").IsInEnum().WithMessage("Enter Valid CityName");
            RuleFor(c => c.CityCode).NotNull().NotEmpty().WithMessage("CityCode is must be required.").MaximumLength(3).WithMessage("required only 3 character");
            RuleFor(c => c.CountryID).NotNull().NotEmpty().WithMessage("CountryID is must be required.");
            RuleFor(c => c.StateID).NotNull().NotEmpty().WithMessage("StateID is must be required.");
        }
    }
}
