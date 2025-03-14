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

    public UserService(IUserRepository userRepository) 
    {
        _userRepository = userRepository;
    }
    public bool IsEighteen(DateOnly birthday)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        int age = currentDate.Year - birthday.Year;
        
        if (currentDate < birthday.AddYears(age))
            age--;
    
        return age >= 18;
    }

    public async Task<bool> CheckRelatedUsersExist(ICollection<UserRelationshipDto> relationships, CancellationToken cancellationToken)
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