namespace Example.Domain.Repositories;

internal interface IFileRepository
{
    long GetFileSize(string filePath);
}
