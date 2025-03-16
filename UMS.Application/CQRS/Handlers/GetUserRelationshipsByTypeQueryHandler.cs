using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using UMS.Application.CQRS.Queries.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;
using UMS.Domain.Resources;
using UMS.Domain.ValueObjects;

namespace UMS.Application.CQRS.Handlers;

public class GetUserRelationshipsByTypeQueryHandler : IRequestHandler<GetUserRelationshipsByTypeQuery, ICollection<UserRelationshipDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRelationshipRepository _relationshipRepository;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public GetUserRelationshipsByTypeQueryHandler(
        IUserRepository userRepository,
        IRelationshipRepository relationshipRepository,
        IStringLocalizer<ErrorMessages> localizer)
    {
        _userRepository = userRepository;
        _relationshipRepository = relationshipRepository;
        _localizer = localizer;
    }

    public async Task<ICollection<UserRelationshipDto>> Handle(GetUserRelationshipsByTypeQuery request, CancellationToken cancellationToken)
    {
        ICollection<UserRelationship> relationships;
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(_localizer[ErrorMessageNames.UserDoesNotExist]);

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
        
        var mappedRelationships = relationships.Select(r => new UserRelationshipDto
        {
            RelatedUserId = r.UserId == request.UserId ? r.RelatedUserId : r.UserId,
            RelationshipType = r.RelationshipType
        }).ToList();

        return mappedRelationships.Adapt<ICollection<UserRelationshipDto>>();
    }
}