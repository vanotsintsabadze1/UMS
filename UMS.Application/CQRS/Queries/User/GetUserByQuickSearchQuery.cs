using MediatR;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Queries.User;

public record GetUserByQuickSearchQuery(string Query, int? Page, int? PageSize) : IRequest<ICollection<UserResponseModel>>;
