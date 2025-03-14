using MediatR;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Queries;

public record GetUserByIdQuery(int UserId) : IRequest<UserResponseModel>;
