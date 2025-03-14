using UMS.Application.Models.User;

namespace UMS.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponseModel> Create(UserRequestModel user, CancellationToken cancellationToken);
    Task<UserResponseModel> ChangeProfileImage(int userId, string fileName, byte[] imageBytes, CancellationToken cancellationToken);
}