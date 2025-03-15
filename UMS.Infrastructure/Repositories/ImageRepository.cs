using Microsoft.Extensions.Configuration;
using UMS.Application.Interfaces.Repositories;

namespace UMS.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private static readonly string _currentDirectory = Directory.GetCurrentDirectory();
    private static readonly string _homeDir = Path.Combine(_currentDirectory, "..");
    private readonly string _fileStoragePath;

    public ImageRepository(IConfiguration configuration)
    {
        var fileStorageSection = configuration.GetSection("FileStorageConfiguration");

        var imageDirectoryParentDirectory = fileStorageSection["ImageDirectoryParentDirectoryName"];
        var imageDirectoryName = fileStorageSection["ImageDirectoryName"];

        _fileStoragePath = Path.Combine(_homeDir, imageDirectoryParentDirectory, imageDirectoryName);
    }
    
    public async Task<bool> SaveFile(string fileName, byte[] fileBytes)
    {
        var filePath = Path.Combine(_fileStoragePath, fileName);
        
        Directory.CreateDirectory(_fileStoragePath);
        
        await File.WriteAllBytesAsync(filePath, fileBytes);

        return true;
    }

    public async Task<Stream> GetFile(string imageUri, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_fileStoragePath, imageUri);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File was not found");

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        return fileStream;
    }
}