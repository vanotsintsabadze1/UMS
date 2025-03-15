using MediatR;
using UMS.Application.Models.User;
using UMS.Domain.Enums;

namespace UMS.Application.CQRS.Commands.User;

public record CreateUserRelationshipCommand(int UserId, int RelatedUserId, UserRelationshipTypes RelationshipType) : IRequest<UserRelationshipDto>;
