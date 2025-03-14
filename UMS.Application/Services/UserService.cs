using Mapster;
using UMS.Application.Exceptions;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Interfaces.Services;
using UMS.Application.Interfaces.UOW;
using UMS.Application.Models.User;
using UMS.Domain.Entities;

namespace UMS.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserResponseModel> Create(UserRequestModel user, CancellationToken cancellationToken)
    {
        var conflictingUser = await _userRepository.GetAsync(us => us.SocialNumber == user.SocialNumber, cancellationToken);
        
        if (conflictingUser is not null)
            throw new ConflictException("User with such social number exists already");

        if (!IsEighteen(user.DateOfBirth))
            throw new BadRequestException("User has to be at least 18 years old");

        var city = await _cityRepository.GetAsync(c => c.Id == user.CityId, cancellationToken);

        if (city is null)
            throw new BadRequestException("Provided city does not exist");
        
        if (user.Relationships is not null)
        {
            var doRelatedUsersExist = await CheckRelatedUsersExist(user.Relationships, cancellationToken);
            
            if (!doRelatedUsersExist)
                throw new BadRequestException("One or more users in the provided relationships do not exist");
        }

        var entity = user.Adapt<User>();
        var response = await _userRepository.AddAsync(entity, cancellationToken);

        return response.Adapt<UserResponseModel>();
    }

    public async Task<UserResponseModel> ChangeProfileImage(int userId, string fileName, byte[] imageBytes, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            // NotFoundException will be added in the future commit.
            // I don't want to clog the first commit with creation of everything
            throw new NotFoundException("User does not exist");
        
        return user.Adapt<UserResponseModel>();
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