using Mapster;
using MediatR;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Handlers;

public class DeleteUserRelationshipCommandHandler : IRequestHandler<DeleteUserRelationshipCommand, UserRelationshipDto>
{
    private IRelationshipRepository _relationshipRepository;

    public DeleteUserRelationshipCommandHandler(IRelationshipRepository relationshipRepository)
    {
        _relationshipRepository = relationshipRepository;
    }

    public async Task<UserRelationshipDto> Handle(DeleteUserRelationshipCommand request,
        CancellationToken cancellationToken)
    {
        var relationship = await _relationshipRepository.GetAsync(
            u => (u.UserId == request.UserId && u.RelatedUserId == request.RelatedUserId) 
                 || (u.UserId == request.RelatedUserId && u.RelatedUserId == request.UserId),
            cancellationToken);

        if (relationship is null)
            throw new NotFoundException("Such relationship does not exist");

        await _relationshipRepository.DeleteAsync(relationship, cancellationToken);
        
        return relationship.Adapt<UserRelationshipDto>();
    }
}