using FluentValidation;
using UMS.Application.Models.User;

namespace UMS.API.Infrastructure.Validators;

public class UserRequestModelValidator : AbstractValidator<UserRequestModel>
{
    public UserRequestModelValidator()
    {
        RuleFor(u => u.Firstname)
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z]+$|^[ა-ჰ]+$")
            .WithMessage("Name must contain only Latin or only Georgian letters.");
        
        RuleFor(u => u.Lastname)
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z]+$|^[ა-ჰ]+$")
            .WithMessage("Name must contain only Latin or only Georgian letters.");

        RuleFor(u => u.SocialNumber)
            .Length(11);
        
    }
}