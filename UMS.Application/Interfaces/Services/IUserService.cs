using UMS.Application.Models.User;

namespace UMS.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponseModel> Update(int userId, UpdateUserRequestModel user, CancellationToken cancellationToken);
}