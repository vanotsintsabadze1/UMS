using MediatR;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Queries.User;

public record GetUserByIdQuery(int UserId) : IRequest<UserResponseModel>;
