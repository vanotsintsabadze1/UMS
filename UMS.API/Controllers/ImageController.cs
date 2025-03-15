using System.Net.Mime;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UMS.API.Models.Response;
using UMS.Application.CQRS.Queries.Image;

namespace UMS.API.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("/api/v{version:apiVersion}/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves the image by it's URI
    /// </summary>
    /// <param name="imageUri"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the image is retrieved successfully</response>
    /// <response code="404">If the image does not exist with such URI</response>
    /// <response code="500">If some unexpected error happened on the server</response>
    [HttpGet("{imageUri}")]
    [Produces(MediaTypeNames.Application.Octet)]
    [ProducesResponseType(typeof(IActionResult), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 500)]
    public async Task<IActionResult> GetImage(string imageUri, CancellationToken cancellationToken)
    {
        var query = new GetImageQuery(imageUri);
        var file= await _mediator.Send(query, cancellationToken);
        var extension = Path.GetExtension(imageUri);
        
        return File(file, $"image/{extension}");
    }
}