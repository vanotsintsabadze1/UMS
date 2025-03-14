using MediatR;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Commands.User;

public record ChangeProfileImageCommand(int UserId, string FileName, byte[] ImageBytes) : IRequest<UserResponseModel>;
