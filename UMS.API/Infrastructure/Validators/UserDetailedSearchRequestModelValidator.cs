using FluentValidation;
using Microsoft.Extensions.Localization;
using UMS.API.Infrastructure.Extensions;
using UMS.Application.Models.User;
using UMS.Domain.Resources;

namespace UMS.API.Infrastructure.Validators;

public class UserDetailedSearchRequestModelValidator : AbstractValidator<UserDetailedSearchRequestModel>
{
    public UserDetailedSearchRequestModelValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(u => u.Firstname)
            .MinimumLength(1)
            .MaximumLength(50)
            .MustBelongToSingleAlphabet()
            .WithMessage(localizer[ErrorMessageNames.PropertyMustBelongTosingleAlphabet]);

        RuleFor(u => u.Lastname)
            .MinimumLength(1)
            .MaximumLength(50)
            .MustBelongToSingleAlphabet()
            .WithMessage(localizer[ErrorMessageNames.PropertyMustBelongTosingleAlphabet]);

        RuleFor(u => u.SocialNumber)
            .MinimumLength(2)
            .When(u => !string.IsNullOrWhiteSpace(u.SocialNumber))
            .WithMessage(localizer[ErrorMessageNames.SocialNumberLengthMustBeEleven])
            .MustBeParsedIntoNumber()
            .When(u => !string.IsNullOrWhiteSpace(u.SocialNumber))
            .WithMessage(localizer[ErrorMessageNames.PropertyMustBeNumberError]);
        
        RuleForEach(u => u.PhoneNumbers)
            .MustBeParsedIntoNumber()
            .WithMessage(localizer[ErrorMessageNames.PropertyMustBeNumberError]);
    }
}