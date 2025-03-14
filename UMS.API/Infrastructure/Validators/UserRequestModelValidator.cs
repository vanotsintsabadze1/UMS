﻿using FluentValidation;
using UMS.API.Infrastructure.Extensions;
using UMS.Application.Models.User;
using UMS.Domain.Enums;

namespace UMS.API.Infrastructure.Validators;

public class UserRequestModelValidator : AbstractValidator<UserRequestModel>
{
    public UserRequestModelValidator()
    {
        RuleFor(u => u.Firstname)
            .MinimumLength(2)
            .MaximumLength(50)
            .MustBelongToSingleAlphabet();

        RuleFor(u => u.Lastname)
            .MinimumLength(2)
            .MaximumLength(50)
            .MustBelongToSingleAlphabet();

        RuleFor(u => u.SocialNumber)
            .Length(11)
            .MustBeParsedIntoNumber();
            
        RuleForEach(u => u.PhoneNumbers)
            .NotEmpty()
            .WithMessage("Phone number can't be empty")
            .Must(u => long.TryParse(u.Number, out _))
            .WithMessage("Phone number has to be a valid number");
    }
}