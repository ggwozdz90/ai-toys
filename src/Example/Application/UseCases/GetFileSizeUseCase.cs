using Example.Domain.Services;

namespace Example.Application.UseCases;

internal interface IGetFileSizeUseCase
{
    long Execute(string filePath);
}

internal sealed class GetFileSizeUseCase(IFileService fileService) : IGetFileSizeUseCase
{
    public long Execute(string filePath)
    {
        return fileService.GetFileSize(filePath);
    }
}
