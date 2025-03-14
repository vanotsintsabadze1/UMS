using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    private readonly IImageRepository _imageRepository;
    private readonly IRelationshipRepository _relationshipRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        IUserRepository userRepository, 
        ICityRepository cityRepository,
        IImageRepository imageRepository,
        IRelationshipRepository relationshipRepository,
        IConfiguration configuration,
        ILogger<UserService> logger,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _cityRepository = cityRepository;
        _relationshipRepository = relationshipRepository;
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserResponseModel> Update(int userId, UpdateUserRequestModel user, CancellationToken cancellationToken)
    {
        var userDb = await _userRepository.GetAsync(u => u.Id == userId, cancellationToken);

        if (userDb is null)
            throw new NotFoundException("Such user does not exist");

        if (!IsEighteen(user.DateOfBirth))
            throw new BadRequestException("Updated user age can't be below 18");

        var city = await _cityRepository.GetAsync(c => c.Id == user.CityId, cancellationToken);

        if (city is null)
            throw new BadRequestException("City with the given id does not exist");
        
        user.Adapt(userDb);

        await _userRepository.UpdateAsync(userDb, cancellationToken);
        return userDb.Adapt<UserResponseModel>();
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