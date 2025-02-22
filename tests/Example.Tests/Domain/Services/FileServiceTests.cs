using Example.Domain.Repositories;
using Example.Domain.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Example.Tests.Domain.Services;

[TestFixture]
internal sealed class FileServiceTests
{
    private IFileRepository fileRepository = null!;
    private FileService fileService = null!;

    [SetUp]
    public void SetUp()
    {
        fileRepository = Substitute.For<IFileRepository>();
        fileService = new FileService(fileRepository);
    }

    [Test]
    public void GetFileSize_WithValidFilePath_ReturnsFileSize()
    {
        // Given
        const string filePath = @"C:\test\file.txt";
        const long expectedFileSize = 12345L;
        fileRepository.GetFileSize(filePath).Returns(expectedFileSize);

        // When
        var result = fileService.GetFileSize(filePath);

        // Then
        result.Should().Be(expectedFileSize);
    }

    [Test]
    public void GetFileSize_WithInvalidFilePath_ThrowsException()
    {
        // Given
        const string filePath = @"C:\invalid\file.txt";
        fileRepository.GetFileSize(filePath).Throws(new FileNotFoundException());

        // When
        Action act = () => fileService.GetFileSize(filePath);

        // Then
        act.Should().Throw<FileNotFoundException>();
    }
}
