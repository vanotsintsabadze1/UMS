using MediatR;

namespace UMS.Application.CQRS.Queries.Image;

public record GetImageQuery(string ImageUri) : IRequest<Stream>;