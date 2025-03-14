using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Interfaces.UOW;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Handlers;

public class ChangeProfileImageCommandHandler : IRequestHandler<ChangeProfileImageCommand, UserResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IImageRepository _imageRepository;
    private readonly ILogger<ChangeProfileImageCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeProfileImageCommandHandler(IUserRepository userRepository, IImageRepository imageRepository, ILogger<ChangeProfileImageCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _imageRepository = imageRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponseModel> Handle(ChangeProfileImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException("User does not exist");

        try
        {
            await _unitOfWork.BeginTransaction(cancellationToken);
            _imageRepository.SaveFile(request.FileName, request.ImageBytes);
            user.ImageUri = request.FileName;
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.CommitTransaction(cancellationToken);
        }
        catch (Exception)
        {
            _logger.LogCritical("Could not save the file - {0}.", request.FileName);
            await _unitOfWork.RollbackTransaction(cancellationToken);
            throw;
        }
        
        return user.Adapt<UserResponseModel>();
    }
}