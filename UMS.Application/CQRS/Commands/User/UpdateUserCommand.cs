using MediatR;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Commands.User;

public record UpdateUserCommand(int UserId, UpdateUserRequestModel User) : IRequest<UserResponseModel>;
