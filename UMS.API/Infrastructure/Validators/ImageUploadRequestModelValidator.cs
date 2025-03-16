using FluentValidation;
using Microsoft.Extensions.Localization;
using UMS.API.Models;
using UMS.Domain.Resources;

namespace UMS.API.Infrastructure.Validators;

public class ImageUploadRequestModelValidator : AbstractValidator<ImageUploadRequestModel>
{
    public ImageUploadRequestModelValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(i => i.Image.ContentType)
            .Matches("image/.*")
            .WithMessage(localizer[ErrorMessageNames.ImageMustBeImage]);
    }
}