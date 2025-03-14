using Mapster;
using MediatR;
using UMS.Application.CQRS.Queries.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseModel>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException("User with the given id does not exist");

        return user.Adapt<UserResponseModel>();
    }
}