using UMS.Application.Models.User;

namespace UMS.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponseModel> Create(UserRequestModel user, CancellationToken cancellationToken);
}