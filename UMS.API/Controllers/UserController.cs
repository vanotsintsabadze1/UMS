using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using UMS.API.Models.Response;
using UMS.Application.Interfaces.Services;
using UMS.Application.Models.User;

namespace UMS.API.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("/api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<ApiResponse<UserResponseModel>> Get([FromBody] UserRequestModel user, CancellationToken cancellationToken)
    {
        var model = await _userService.Create(user, cancellationToken);
        return new ApiResponse<UserResponseModel>(model);
    }
}
