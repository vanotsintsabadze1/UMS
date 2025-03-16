using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Interfaces.Services;
using UMS.Application.Models.User;
using UMS.Domain.Resources;

namespace UMS.Application.CQRS.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IUserService _userService;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    
    public UpdateUserCommandHandler
    (IUserRepository userRepository,
        ICityRepository cityRepository,
        IUserService userService,
        IStringLocalizer<ErrorMessages> localizer)
    {
        _userRepository = userRepository;
        _cityRepository = cityRepository;
        _userService = userService;
        _localizer = localizer;
    }

    public async Task<UserResponseModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userDb = await _userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);

        if (userDb is null)
            throw new NotFoundException(_localizer[ErrorMessageNames.UserDoesNotExist]);

        var requestPhoneNumbers = request.User.PhoneNumbers.Select(x => x.Number).ToList();

        var conflictingUser = await _userRepository.GetAsync(u => 
                u.Id != request.UserId && // Exclude the current user
                (
                    u.SocialNumber == request.User.SocialNumber ||
                    u.PhoneNumbers.Any(pn => requestPhoneNumbers.Contains(pn.Number))
                ),
            cancellationToken
        );

        if (conflictingUser is not null)
            throw new ConflictException(_localizer[ErrorMessageNames.UserAlreadyExistsWithSocialNumberOrPhoneNumber]);
        
        if (!_userService.IsEighteen(request.User.DateOfBirth))
            throw new BadRequestException(_localizer[ErrorMessageNames.UserMustBeAtLeast18YearsOld]);

        var city = await _cityRepository.GetAsync(c => c.Id == request.User.CityId, cancellationToken);

        if (city is null)
            throw new BadRequestException(_localizer[ErrorMessageNames.ProvidedCityDoesNotExist]);
        
        request.User.Adapt(userDb);

        await _userRepository.UpdateAsync(userDb, cancellationToken);
        return userDb.Adapt<UserResponseModel>();   
    }
}