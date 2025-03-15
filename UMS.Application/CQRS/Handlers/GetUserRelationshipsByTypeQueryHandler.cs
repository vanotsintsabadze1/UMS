using Mapster;
using MediatR;
using UMS.Application.CQRS.Queries.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;
using UMS.Domain.ValueObjects;

namespace UMS.Application.CQRS.Handlers;

public class GetUserRelationshipsByTypeQueryHandler : IRequestHandler<GetUserRelationshipsByTypeQuery, ICollection<UserRelationshipDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRelationshipRepository _relationshipRepository;
    
    public GetUserRelationshipsByTypeQueryHandler(IUserRepository userRepository, IRelationshipRepository relationshipRepository)
    {
        _userRepository = userRepository;
        _relationshipRepository = relationshipRepository;
    }

    public async Task<ICollection<UserRelationshipDto>> Handle(GetUserRelationshipsByTypeQuery request, CancellationToken cancellationToken)
    {
        ICollection<UserRelationship> relationships;
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException("User with the given id does not exist");

        if (request.RelationshipType is not null)
        {
            relationships = await _relationshipRepository.GetAllAsync(
                    u => (u.UserId == request.UserId || u.RelatedUserId == request.UserId) && u.RelationshipType == request.RelationshipType,
                    cancellationToken);
        }
        else
        {
            relationships = await _relationshipRepository.GetAllAsync(
                u => (u.UserId == request.UserId || u.RelatedUserId == request.UserId),
                cancellationToken);
        }
        

        return relationships.Adapt<ICollection<UserRelationshipDto>>();
    }
}