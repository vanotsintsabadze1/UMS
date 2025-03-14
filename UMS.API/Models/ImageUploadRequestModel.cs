namespace UMS.API.Models;

public class ImageUploadRequestModel
{
    public required int UserId { get; set; }
    public required IFormFile Image { get; set; }
}