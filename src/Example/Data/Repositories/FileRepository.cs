using System.IO.Abstractions;
using Example.Domain.Repositories;

namespace Example.Data.Repositories;

internal sealed class FileRepository(IFileSystem fileSystem) : IFileRepository
{
    public long GetFileSize(string filePath)
    {
        var fileInfo = fileSystem.FileInfo.New(filePath);
        return fileInfo.Length;
    }
}
