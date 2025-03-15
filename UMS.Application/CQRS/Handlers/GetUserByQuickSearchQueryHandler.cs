using Mapster;
using MediatR;
using UMS.Application.CQRS.Queries.User;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.Response;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Handlers;

public class GetUserByQuickSearchQueryHandler : IRequestHandler<GetUserByQuickSearchQuery, PaginatedResponseModel<ICollection<UserResponseModel>>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByQuickSearchQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<PaginatedResponseModel<ICollection<UserResponseModel>>> Handle(GetUserByQuickSearchQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return new PaginatedResponseModel<ICollection<UserResponseModel>>()
            {
                Items = new List<UserResponseModel>(),
                TotalCount = 0
            };
        }

        var (users, totalCount) = await _userRepository.GetUserByQueryLike(
            request.Query,
            request.Page ?? 1,
            request.PageSize ?? 10,
            cancellationToken);

        var items = users.Adapt<ICollection<UserResponseModel>>();
        
        return new PaginatedResponseModel<ICollection<UserResponseModel>>()
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}