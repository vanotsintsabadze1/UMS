using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;
using UMS.Domain.Resources;

namespace UMS.Application.CQRS.Handlers;

public class DeleteUserRelationshipCommandHandler : IRequestHandler<DeleteUserRelationshipCommand, UserRelationshipDto>
{
    private IRelationshipRepository _relationshipRepository;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    
    public DeleteUserRelationshipCommandHandler(IRelationshipRepository relationshipRepository, IStringLocalizer<ErrorMessages> localizer)
    {
        _relationshipRepository = relationshipRepository;
        _localizer = localizer;
    }

    public async Task<UserRelationshipDto> Handle(DeleteUserRelationshipCommand request,
        CancellationToken cancellationToken)
    {
        var relationship = await _relationshipRepository.GetAsync(
            u => (u.UserId == request.UserId && u.RelatedUserId == request.RelatedUserId) 
                 || (u.UserId == request.RelatedUserId && u.RelatedUserId == request.UserId),
            cancellationToken);

        if (relationship is null)
            throw new NotFoundException(_localizer[ErrorMessageNames.RelationshipDoesNotExist]);

        await _relationshipRepository.DeleteAsync(relationship, cancellationToken);
        
        return relationship.Adapt<UserRelationshipDto>();
    }
}