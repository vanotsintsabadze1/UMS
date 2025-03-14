using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Interfaces.UOW;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IRelationshipRepository _relationshipRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(
        IUserRepository userRepository,
        IRelationshipRepository relationshipRepository, 
        ILogger<DeleteUserCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _relationshipRepository = relationshipRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserResponseModel> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException("User with such id does not exist");

        var indirectUserRelationships = await _relationshipRepository.GetAllAsync(r => r.RelatedUserId == request.UserId, cancellationToken);

        if (indirectUserRelationships.Count != 0)
            await _relationshipRepository.RemoveRangeAsync(indirectUserRelationships, cancellationToken);

        var response = user.Adapt<UserResponseModel>();

        try
        {
            await _unitOfWork.BeginTransaction(cancellationToken);
       
            if (user.Relationships is not null)
                await _relationshipRepository.RemoveRangeAsync(user.Relationships, cancellationToken);
            
            await _userRepository.DeleteAsync(user, cancellationToken);
            await _unitOfWork.CommitTransaction(cancellationToken);
        }
        catch (Exception)
        {
            _logger.LogCritical("Critical error occured while trying to delete a user - {0}", user);
            await _unitOfWork.RollbackTransaction(cancellationToken);
            throw;
        }

        return response;    }
}