using System.Net.Mime;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using UMS.API.Infrastructure.Utilities;
using UMS.API.Models;
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
    
    /// <summary>
    /// Creates the user 
    /// </summary>
    /// <remarks>
    /// <b>Gender types can be</b>: Male, Female    
    /// \
    /// <b>Mobile phone types can be</b>: Mobile, Office, Home
    /// \
    /// \
    /// <b>Relationship types can be</b>: Colleague, Friend, Relative, Other
    /// </remarks>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the user was successfully created</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code ="409">If the user already exists</response> 
    /// <response code="500">If some unexpected error happened on the server</response>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 409)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    [HttpPost]
    public async Task<ApiResponse<UserResponseModel>> Create([FromBody] UserRequestModel user, CancellationToken cancellationToken)
    {
        var model = await _userService.Create(user, cancellationToken);
        return new ApiResponse<UserResponseModel>(model);
    }

    /// <summary>
    /// Uploads/Changes the profile image for the user
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the image was uploaded successfully</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code="401">If the user was not found</response>
    /// <response code="500">If something went wrong on the server</response>
    [HttpPatch("image")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 401)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ApiResponse<UserResponseModel>> UploadProfileImage([FromForm] ImageUploadRequestModel model, CancellationToken cancellationToken)
    {
        var imageBytes = await FileUtility.ConvertToByteArray(model.Image);
        var response = await _userService.ChangeProfileImage(model.UserId, model.Image.FileName, imageBytes, cancellationToken);

        return new ApiResponse<UserResponseModel>(response);
    }
}
