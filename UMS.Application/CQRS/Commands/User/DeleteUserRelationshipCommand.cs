using MediatR;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Commands.User;

public record DeleteUserRelationshipCommand(int UserId, int RelatedUserId) : IRequest<UserRelationshipDto>;
