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
    
    public async Task<UserResponseModel> ChangeProfileImage(int userId, string fileName, byte[] imageBytes, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            throw new NotFoundException("User does not exist");

        try
        {
            await _unitOfWork.BeginTransaction(cancellationToken);
            _imageRepository.SaveFile(fileName, imageBytes);
            user.ImageUri = fileName;
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.CommitTransaction(cancellationToken);
        }
        catch (Exception)
        {
            _logger.LogCritical("Could not save the file - {0}.", fileName);
            await _unitOfWork.RollbackTransaction(cancellationToken);
            throw;
        }
        
        return user.Adapt<UserResponseModel>();
    }

    public async Task<UserResponseModel> Delete(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            throw new NotFoundException("User with such id does not exist");

        var indirectUserRelationships = await _relationshipRepository.GetAllAsync(r => r.RelatedUserId == userId, cancellationToken);

        if (indirectUserRelationships.Count != 0)
            await _relationshipRepository.RemoveRangeAsync(indirectUserRelationships, cancellationToken);

        var response = user.Adapt<UserResponseModel>();

        try
        {
            await _unitOfWork.BeginTransaction(cancellationToken);
       
            if (user.Relationships is not null)
                await _relationshipRepository.RemoveRangeAsync(user.Relationships, cancellationToken);
            
            await _userRepository.DeleteAsync(user, cancellationToken);
            await _unitOfWork.CommitTransaction(cancellationToken);
        }
        catch (Exception)
        {
            _logger.LogCritical("Critical error occured while trying to delete a user - {0}", user);
            await _unitOfWork.RollbackTransaction(cancellationToken);
            throw;
        }

        return response;
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