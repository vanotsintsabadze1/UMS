using Mapster;
using MediatR;
using UMS.Application.CQRS.Queries.User;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Handlers;

public class GetUserByQuickSearchQueryHandler : IRequestHandler<GetUserByQuickSearchQuery, ICollection<UserResponseModel>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByQuickSearchQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<ICollection<UserResponseModel>> Handle(GetUserByQuickSearchQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return new List<UserResponseModel>();

        var users = await _userRepository.GetUserByQueryLike(
            request.Query,
            request.Page ?? 1,
            request.PageSize ?? 10,
            cancellationToken);

        return users.Adapt<ICollection<UserResponseModel>>();
    }
}