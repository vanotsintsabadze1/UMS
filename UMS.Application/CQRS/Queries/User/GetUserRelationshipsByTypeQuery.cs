using MediatR;
using UMS.Application.Models.User;
using UMS.Domain.Enums;

namespace UMS.Application.CQRS.Queries.User;

public record GetUserRelationshipsByTypeQuery(int UserId, UserRelationshipTypes? RelationshipType) : IRequest<ICollection<UserRelationshipDto>>;