using Mapster;
using MediatR;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;
using UMS.Domain.ValueObjects;

namespace UMS.Application.CQRS.Handlers;

public class CreateUserRelationshipCommandHandler : IRequestHandler<CreateUserRelationshipCommand, UserRelationshipDto>
{
    private readonly IRelationshipRepository _relationshipRepository;
    private readonly IUserRepository _userRepository;
    
    public CreateUserRelationshipCommandHandler(IRelationshipRepository relationshipRepository, IUserRepository userRepository)
    {
        _relationshipRepository = relationshipRepository;
        _userRepository = userRepository;
    }

    public async Task<UserRelationshipDto> Handle(CreateUserRelationshipCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);
        var relatedUser = await _userRepository.GetAsync(u => u.Id == request.RelatedUserId, cancellationToken);

        if (user is null || relatedUser is null)
            throw new NotFoundException("User or related user does not exist");

        var existingRelationship = await _relationshipRepository.GetAsync(r =>
                (r.UserId == request.UserId && r.RelatedUserId == request.RelatedUserId)
                || (r.UserId == request.RelatedUserId && r.RelatedUserId == request.UserId),
            cancellationToken);

        if (existingRelationship is not null)
            throw new ConflictException("Relationship between these users exist already");

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