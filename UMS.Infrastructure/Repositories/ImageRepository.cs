using Microsoft.Extensions.Configuration;
using UMS.Application.Interfaces.Repositories;

namespace UMS.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly string _currentDirectory = Directory.GetCurrentDirectory();
    private readonly string _imageDirectoryParentDirectory;
    private readonly string _imageDirectoryName;

    public ImageRepository(IConfiguration configuration)
    {
        var section = configuration.GetSection("FileStorageConfiguration");
        _imageDirectoryParentDirectory = section["ImageDirectoryParentDirectoryName"];
        _imageDirectoryName = section["ImageDirectoryName"];
    }
    
    public async Task<bool> SaveFile(string fileName, byte[] fileBytes)
    {
        var homeDir = Path.Combine(_currentDirectory, "..");
        var fileStoragePath = Path.Combine(homeDir, _imageDirectoryParentDirectory, _imageDirectoryName);
        var filePath = Path.Combine(fileStoragePath, fileName);
        
        Directory.CreateDirectory(fileStoragePath);
        
        await File.WriteAllBytesAsync(filePath, fileBytes);

        return true;
    }
}