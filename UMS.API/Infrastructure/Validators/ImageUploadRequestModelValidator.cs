using FluentValidation;
using UMS.API.Models;

namespace UMS.API.Infrastructure.Validators;

public class ImageUploadRequestModelValidator : AbstractValidator<ImageUploadRequestModel>
{
    public ImageUploadRequestModelValidator()
    {
        RuleFor(i => i.Image.ContentType)
            .Matches("image/.*")
            .WithMessage("Only images are allowed to be uploaded");
    }
}