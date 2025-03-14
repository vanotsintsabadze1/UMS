﻿using UMS.Application.Models.User;

namespace UMS.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponseModel> ChangeProfileImage(int userId, string fileName, byte[] imageBytes, CancellationToken cancellationToken);
    Task<UserResponseModel> Delete(int userId, CancellationToken cancellationToken);
    Task<UserResponseModel> Update(int userId, UpdateUserRequestModel user, CancellationToken cancellationToken);
}