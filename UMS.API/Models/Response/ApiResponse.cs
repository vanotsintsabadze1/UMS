namespace UMS.API.Models.Response;

public record ApiResponse(string Message, Dictionary<string, string[]>? Errors = null);
public record ApiResponse<T>(T Data);