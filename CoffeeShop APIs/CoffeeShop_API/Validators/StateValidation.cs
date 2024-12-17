using CoffeeShop_API.Models;
using FluentValidation;

namespace DemoWebAPI.Validators
{
    public class StateValidation : AbstractValidator<StateModel>
    {
        public StateValidation() 
        {
            RuleFor(s => s.StateName).NotNull().NotEmpty().WithMessage("StateName is must be required.").IsInEnum().WithMessage("Invalid Input. please Enter A Valid String.");
            RuleFor(s => s.StateCode).NotNull().NotEmpty().WithMessage("StateCode is must be required.").MaximumLength(3).WithMessage("required only 3 character"); ;
            RuleFor(s => s.CountryID).NotNull().NotEmpty().WithMessage("StateName is must be required.");
        }
    }
}
