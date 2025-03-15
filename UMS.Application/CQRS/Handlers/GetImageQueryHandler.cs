using MediatR;
using UMS.Application.CQRS.Queries.Image;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;

namespace UMS.Application.CQRS.Handlers;

public class GetImageQueryHandler : IRequestHandler<GetImageQuery, Stream>
{
    private readonly IImageRepository _imageRepository;

    public GetImageQueryHandler(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
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
            throw new NotFoundException("File was not found");
        }
    }
}