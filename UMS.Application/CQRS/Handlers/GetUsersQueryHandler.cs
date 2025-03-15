using Mapster;
using MediatR;
using UMS.Application.CQRS.Queries.User;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.Response;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Handlers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedResponseModel<ICollection<UserResponseModel>>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<PaginatedResponseModel<ICollection<UserResponseModel>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _userRepository.GetUsersPaginated(
            request.User,
            request.Page ?? 1,
            request.PageSize ?? 10,
            cancellationToken);

        var users = items.Adapt<ICollection<UserResponseModel>>();

        return new PaginatedResponseModel<ICollection<UserResponseModel>>()
        {
            Items = users,
            TotalCount = totalCount
        };
    }
}