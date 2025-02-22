using Example.Application.UseCases;

namespace Example.Adapter.Adapters;

/// <summary>
/// Interface for file adapter to get file size.
/// </summary>
public interface IFileAdapter
{
    /// <summary>
    /// Gets the size of the file at the specified path.
    /// </summary>
    /// <param name="filePath">The path of the file.</param>
    /// <returns>The size of the file in bytes.</returns>
    long GetFileSize(string filePath);
}

internal sealed class FileAdapter(IGetFileSizeUseCase getFileSizeUseCase) : IFileAdapter
{
    public long GetFileSize(string filePath)
    {
        return getFileSizeUseCase.Execute(filePath);
    }
}
