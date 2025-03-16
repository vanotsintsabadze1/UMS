using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;
using UMS.Domain.Resources;
using UMS.Domain.ValueObjects;

namespace UMS.Application.CQRS.Handlers;

public class CreateUserRelationshipCommandHandler : IRequestHandler<CreateUserRelationshipCommand, UserRelationshipDto>
{
    private readonly IRelationshipRepository _relationshipRepository;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    private readonly IUserRepository _userRepository;
    
    public CreateUserRelationshipCommandHandler(
        IRelationshipRepository relationshipRepository,
        IUserRepository userRepository,
        IStringLocalizer<ErrorMessages> localizer)
    {
        _relationshipRepository = relationshipRepository;
        _localizer = localizer;
        _userRepository = userRepository;
    }

    public async Task<UserRelationshipDto> Handle(CreateUserRelationshipCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);
        var relatedUser = await _userRepository.GetAsync(u => u.Id == request.RelatedUserId, cancellationToken);

        if (user is null || relatedUser is null)
            throw new NotFoundException(_localizer[ErrorMessageNames.UsersInRelationshipDoNotExist]);

        var existingRelationship = await _relationshipRepository.GetAsync(r =>
                (r.UserId == request.UserId && r.RelatedUserId == request.RelatedUserId)
                || (r.UserId == request.RelatedUserId && r.RelatedUserId == request.UserId),
            cancellationToken);

        if (existingRelationship is not null)
            throw new ConflictException(_localizer[ErrorMessageNames.RelationshipExistsAlready]);

        var relationship = new UserRelationship()
        {
            UserId = request.UserId,
            RelatedUserId = request.RelatedUserId,
            RelationshipType = request.RelationshipType,
        };

        await _relationshipRepository.AddAsync(relationship, cancellationToken);
        return relationship.Adapt<UserRelationshipDto>();
    }
}