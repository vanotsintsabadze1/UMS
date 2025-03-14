namespace UMS.API.Infrastructure.Utilities;

public static class FileUtility
{
    public static async Task<byte[]> ConvertToByteArray(IFormFile file)
    {
        byte[] fileBytes;
        
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
        
        return fileBytes;
    }
}