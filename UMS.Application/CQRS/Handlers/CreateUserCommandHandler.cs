using Mapster;
using MediatR;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;
using UMS.Domain.Entities;

namespace UMS.Application.CQRS.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly ICityRepository _cityRepository;

    public CreateUserCommandHandler(IUserRepository userRepository, ICityRepository cityRepository)
    {
        _userRepository = userRepository;
        _cityRepository = cityRepository;
    }
    
    public async Task<UserResponseModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var conflictingUser = await _userRepository.GetAsync(us => us.SocialNumber == request.User.SocialNumber, cancellationToken);
        
        if (conflictingUser is not null)
            throw new ConflictException("User with such social number exists already");

        if (!IsEighteen(request.User.DateOfBirth))
            throw new BadRequestException("User has to be at least 18 years old");

        var city = await _cityRepository.GetAsync(c => c.Id == request.User.CityId, cancellationToken);

        if (city is null)
            throw new BadRequestException("Provided city does not exist");
        
        if (request.User.Relationships is not null)
        {
            var doRelatedUsersExist = await CheckRelatedUsersExist(request.User.Relationships, cancellationToken);
            
            if (!doRelatedUsersExist)
                throw new BadRequestException("One or more users in the provided relationships do not exist");
        }

        var entity = request.User.Adapt<User>();
        var response = await _userRepository.AddAsync(entity, cancellationToken);

        return response.Adapt<UserResponseModel>();
    }
    
    private bool IsEighteen(DateOnly birthday)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        int age = currentDate.Year - birthday.Year;
        
        if (currentDate < birthday.AddYears(age))
            age--;
    
        return age >= 18;
    }

    private async Task<bool> CheckRelatedUsersExist(ICollection<UserRelationshipDto> relationships, CancellationToken cancellationToken)
    {
        foreach (var relationship in relationships)
        {
            var user = await _userRepository.GetAsync(u => u.Id == relationship.RelatedUserId, cancellationToken);

            if (user is null)
                return false;
        }
        
        return true;
    }
}