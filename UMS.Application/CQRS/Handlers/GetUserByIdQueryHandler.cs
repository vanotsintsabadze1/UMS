using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using UMS.Application.CQRS.Queries.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;
using UMS.Domain.Resources;

namespace UMS.Application.CQRS.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    
    public GetUserByIdQueryHandler(IUserRepository userRepository, IStringLocalizer<ErrorMessages> localizer)
    {
        _userRepository = userRepository;
        _localizer = localizer;
    }

    public async Task<UserResponseModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(_localizer[ErrorMessageNames.UserDoesNotExist]);

        return user.Adapt<UserResponseModel>();
    }
}