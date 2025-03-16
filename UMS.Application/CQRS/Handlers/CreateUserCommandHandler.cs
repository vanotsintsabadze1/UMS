using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Interfaces.Services;
using UMS.Application.Models.User;
using UMS.Domain.Entities;
using UMS.Domain.Resources;

namespace UMS.Application.CQRS.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    private readonly IUserService _userService;
    
    public CreateUserCommandHandler(
        IUserRepository userRepository,
        ICityRepository cityRepository,
        IUserService userService,
        IStringLocalizer<ErrorMessages> localizer)
    {
        _userRepository = userRepository;
        _cityRepository = cityRepository;
        _localizer = localizer;
        _userService = userService;
    }
    
    public async Task<UserResponseModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userWithConflictingSocialNumber = await _userRepository.GetAsync(us => us.SocialNumber == request.User.SocialNumber, cancellationToken);
        
        if (userWithConflictingSocialNumber is not null)
            throw new ConflictException(_localizer[ErrorMessageNames.UserAlreadyExistsWithSocialNumber]);

        var requestPhoneNumbers = request.User.PhoneNumbers.Select(u => u.Number).ToList();
        var userWithConflictingPhoneNumber = await _userRepository.GetAsync(
            us => us.PhoneNumbers.Any(pn => requestPhoneNumbers.Contains(pn.Number)),
            cancellationToken);

        if (userWithConflictingPhoneNumber is not null)
            throw new ConflictException(_localizer[ErrorMessageNames.UserWithPhoneNumberExists]);
        
        if (!_userService.IsEighteen(request.User.DateOfBirth))
            throw new BadRequestException(_localizer[ErrorMessageNames.UserMustBeAtLeast18YearsOld]);

        var city = await _cityRepository.GetAsync(c => c.Id == request.User.CityId, cancellationToken);

        if (city is null)
            throw new BadRequestException(_localizer[ErrorMessageNames.ProvidedCityDoesNotExist]);
        
        if (request.User.Relationships is not null)
        {
            var doRelatedUsersExist = await _userService.CheckRelatedUsersExist(request.User.Relationships, cancellationToken);
            
            if (!doRelatedUsersExist)
                throw new BadRequestException(_localizer[ErrorMessageNames.UsersInRelationshipDoNotExist]);
        }

        var entity = request.User.Adapt<User>();
        var response = await _userRepository.AddAsync(entity, cancellationToken);

        return response.Adapt<UserResponseModel>();
    }
}