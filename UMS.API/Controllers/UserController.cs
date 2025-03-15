using System.Net.Mime;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UMS.API.Infrastructure.Utilities;
using UMS.API.Models;
using UMS.API.Models.Response;
using UMS.Application.CQRS.Commands.User;
using UMS.Application.CQRS.Queries.User;
using UMS.Application.Models.Response;
using UMS.Application.Models.User;
using UMS.Domain.Enums;

namespace UMS.API.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("/api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets the user by quick search query
    /// </summary>
    /// <param name="searchQuery"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the query was successful</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code="500">If some unexpected error happened on the server</response>
    [HttpGet("search", Name = "GetUsersByQuickSearch")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), 200)]
    public async Task<ApiResponse<PaginatedResponseModel<ICollection<UserResponseModel>>>> GetByQuickSearch([FromQuery] string searchQuery, int? page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = new GetUserByQuickSearchQuery(searchQuery, page, pageSize);
        var response = await _mediator.Send(query, cancellationToken);
        return new ApiResponse<PaginatedResponseModel<ICollection<UserResponseModel>>>(response);
    }

    /// <summary>
    /// Gets the users by detailed search if query params exist, otherwise gets all users paginated
    /// </summary>
    /// <remarks>
    /// If you call this endpoint without any query parameters, then all the users will be retrieved in a paginated manner
    /// </remarks>
    /// <param name="user"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the request was successful</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code="500">If some unexpected error happened on the server</response>
    [HttpGet]
    [Route("/api/v{version:apiVersion}/users")]
    public async Task<ApiResponse<PaginatedResponseModel<ICollection<UserResponseModel>>>> GetUsersPaginated([FromQuery] UserDetailedSearchRequestModel user, int? page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(user, page, pageSize);
        var response = await _mediator.Send(query, cancellationToken);
        return new ApiResponse<PaginatedResponseModel<ICollection<UserResponseModel>>>(response);
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
        var command = new CreateUserCommand(user);
        var response = await _mediator.Send(command, cancellationToken);
        return new ApiResponse<UserResponseModel>(response);
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

        var command = new ChangeProfileImageCommand(model.UserId, model.Image.FileName, imageBytes);
        var response = await _mediator.Send(command, cancellationToken);

        return new ApiResponse<UserResponseModel>(response);
    }

    /// <summary>
    /// Deletes the existing user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the user was deleted successfully</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code="401">If the user was not found</response>
    /// <response code="500">If something went wrong on the server</response>
    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 401)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ApiResponse<UserResponseModel>> Delete(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        var response = await _mediator.Send(command, cancellationToken);
        return new ApiResponse<UserResponseModel>(response);
    }

    /// <summary>
    /// Updates the existing user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the user was updated successfully</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code="401">If the user was not found</response>
    /// <response code="500">If something went wrong on the server</response>
    [HttpPut("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 401)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ApiResponse<UserResponseModel>> Update([FromRoute] int id, [FromBody] UpdateUserRequestModel user, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(id, user);
        var response = await _mediator.Send(command, cancellationToken);
        return new ApiResponse<UserResponseModel>(response);
    }

    /// <summary>
    /// Retrieves the user by their id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the user was retrieved successfully</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code="401">If the user was not found</response>
    /// <response code="500">If something went wrong on the server</response>
    [HttpGet("{id}")]   
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 401)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ApiResponse<UserResponseModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var response = await _mediator.Send(query, cancellationToken);
        return new ApiResponse<UserResponseModel>(response);
    }
    
    /// <summary>
    /// Gets the user's relationships based on the type
    /// </summary>
    /// <remarks><b>Note:</b> If the relationship type is not passed, then all the relationships will be grabbed</remarks>
    /// <param name="userId"></param>
    /// <param name="type"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the user relationships were retrieved successfully</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code="404">If the user was not found</response>
    /// <response code="500">If something went wrong on the server</response>
    [HttpGet("relationships")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ApiResponse<ICollection<UserRelationshipDto>>> GetRelationships(int userId, UserRelationshipTypes? type, CancellationToken cancellationToken)
    {
        var query = new GetUserRelationshipsByTypeQuery(userId, type);
        var response = await _mediator.Send(query, cancellationToken);
        return new ApiResponse<ICollection<UserRelationshipDto>>(response);
    }
    
    /// <summary>
    /// Deletes a user relationship based on the user id and the related user id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="relatedUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the user relationship was deleted successfully</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code="404">If the relationship was not found</response>
    /// <response code="500">If something went wrong on the server</response>
    [HttpDelete("relationship")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ApiResponse<UserRelationshipDto>> DeleteRelationships(int userId, int relatedUserId, CancellationToken cancellationToken)
    {
        var command = new DeleteUserRelationshipCommand(userId, relatedUserId);
        var response = await _mediator.Send(command, cancellationToken);
        return new ApiResponse<UserRelationshipDto>(response);
    }
    
        
    /// <summary>
    /// Creates a relationship between two users
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="relatedUserId"></param>
    /// <param name=""></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the user relationship was deleted successfully</response>
    /// <response code="400">If the data in the request was invalid</response>
    /// <response code="404">If single or both users were not found</response>
    /// <response code="409">If the relationship already exists</response>
    /// <response code="500">If something went wrong on the server</response>
    [HttpPatch("relationship")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 409)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<ApiResponse<UserRelationshipDto>> CreateRelationship(int userId, int relatedUserId, UserRelationshipTypes userRelationshipType, CancellationToken cancellationToken)
    {
        var command = new CreateUserRelationshipCommand(userId, relatedUserId, userRelationshipType);
        var response = await _mediator.Send(command, cancellationToken);
        return new ApiResponse<UserRelationshipDto>(response);
    }
}