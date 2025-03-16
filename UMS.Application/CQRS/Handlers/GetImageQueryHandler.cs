using MediatR;
using Microsoft.Extensions.Localization;
using UMS.Application.CQRS.Queries.Image;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Domain.Resources;

namespace UMS.Application.CQRS.Handlers;

public class GetImageQueryHandler : IRequestHandler<GetImageQuery, Stream>
{
    private readonly IImageRepository _imageRepository;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    
    public GetImageQueryHandler(IImageRepository imageRepository, IStringLocalizer<ErrorMessages> localizer)
    {
        _imageRepository = imageRepository;
        _localizer = localizer;
    }

    public async Task<Stream> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var file = await _imageRepository.GetFile(request.ImageUri, cancellationToken);
            return file;
        }
        catch (FileNotFoundException exception)
        {
            throw new NotFoundException(_localizer[ErrorMessageNames.FileDoesNotExist]);
        }
    }
}