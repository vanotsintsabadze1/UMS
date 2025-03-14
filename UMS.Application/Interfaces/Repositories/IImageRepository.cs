namespace UMS.Application.Interfaces.Repositories;

public interface IImageRepository
{
    Task<bool> SaveFile(string fileName, byte[] fileBytes);
}