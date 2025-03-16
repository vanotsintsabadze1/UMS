using FluentValidation;
using Microsoft.Extensions.Localization;
using UMS.API.Infrastructure.Extensions;
using UMS.Application.Models.User;
using UMS.Domain.Resources;

namespace UMS.API.Infrastructure.Validators;

public class UserRequestModelValidator : AbstractValidator<UserRequestModel>
{
    public UserRequestModelValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(u => u.Firstname)
            .MinimumLength(2)
            .MaximumLength(50)
            .MustBelongToSingleAlphabet()
            .WithMessage(localizer[ErrorMessageNames.PropertyMustBelongTosingleAlphabet]);

        RuleFor(u => u.Lastname)
            .MinimumLength(2)
            .MaximumLength(50)
            .MustBelongToSingleAlphabet()
            .WithMessage(localizer[ErrorMessageNames.PropertyMustBelongTosingleAlphabet]);

        RuleFor(u => u.SocialNumber)
            .Length(11)
            .WithMessage(localizer[ErrorMessageNames.SocialNumberLengthMustBeEleven])
            .MustBeParsedIntoNumber()
            .WithMessage((_, _) => localizer[ErrorMessageNames.PropertyMustBeNumberError]);

        RuleForEach(u => u.PhoneNumbers)
            .NotEmpty()
                .WithMessage(localizer[ErrorMessageNames.PhoneNumberCanNotBeEmpty])
            .Must(u => long.TryParse(u.Number, out _))
            .WithMessage(localizer[ErrorMessageNames.PropertyMustBeNumberError])
            .Must((parent, item, context) =>
                parent.PhoneNumbers.Count(pn => pn.Number == item.Number) == 1)
            .WithMessage(localizer[ErrorMessageNames.PhoneNumberCantBeDuplicate]);
    }
}