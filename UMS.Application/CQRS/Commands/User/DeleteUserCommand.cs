using MediatR;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Commands.User;

public record DeleteUserCommand(int UserId) : IRequest<UserResponseModel>;
