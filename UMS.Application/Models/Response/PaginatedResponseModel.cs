namespace UMS.Application.Models.Response;

public class PaginatedResponseModel<TModel>
{
    public required TModel Items { get; set; }
    public required int TotalCount { get; set; }
}