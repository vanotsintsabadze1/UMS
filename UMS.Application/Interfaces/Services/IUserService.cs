using UMS.Application.Models.User;

namespace UMS.Application.Interfaces.Services;

public interface IUserService
{
    bool IsEighteen(DateOnly birthday);

    Task<bool> CheckRelatedUsersExist(ICollection<UserRelationshipDto> relationships, CancellationToken cancellationToken);
}