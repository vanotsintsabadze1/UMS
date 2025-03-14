using Mapster;
using MediatR;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Interfaces.Services;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IUserService _userService;
    
    public UpdateUserCommandHandler(IUserRepository userRepository, ICityRepository cityRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _cityRepository = cityRepository;
        _userService = userService;
    }

    public async Task<UserResponseModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userDb = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);

        if (userDb is null)
            throw new NotFoundException("Such user does not exist");

        if (!_userService.IsEighteen(request.User.DateOfBirth))
            throw new BadRequestException("Updated user age can't be below 18");

        var city = await _cityRepository.GetAsync(c => c.Id == request.User.CityId, cancellationToken);

        if (city is null)
            throw new BadRequestException("City with the given id does not exist");
        
        request.User.Adapt(userDb);

        await _userRepository.UpdateAsync(userDb, cancellationToken);
        return userDb.Adapt<UserResponseModel>();   
    }
}