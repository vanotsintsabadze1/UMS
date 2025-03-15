using MediatR;
using UMS.Application.Models.Response;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Queries.User;

public record GetUsersQuery(UserDetailedSearchRequestModel User, int? Page, int? PageSize) : IRequest<PaginatedResponseModel<ICollection<UserResponseModel>>>;
