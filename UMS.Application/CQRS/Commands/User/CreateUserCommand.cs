﻿using MediatR;
using UMS.Application.Models.User;

namespace UMS.Application.CQRS.Commands.User;

public record CreateUserCommand(UserRequestModel User) : IRequest<UserResponseModel>;
