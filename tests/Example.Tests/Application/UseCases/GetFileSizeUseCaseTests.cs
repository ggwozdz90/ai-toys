using Example.Application.UseCases;
using Example.Domain.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Example.Tests.Application.UseCases;

[TestFixture]
internal sealed class GetFileSizeUseCaseTests
{
    private IFileService fileService = null!;
    private GetFileSizeUseCase getFileSizeUseCase = null!;

    [SetUp]
    public void Setup()
    {
        fileService = Substitute.For<IFileService>();
        getFileSizeUseCase = new GetFileSizeUseCase(fileService);
    }

    [Test]
    public void Execute_WithValidFilePath_ReturnsFileSize()
    {
        // Given
        const string filePath = "validFilePath.txt";
        const long expectedSize = 1024L;
        fileService.GetFileSize(filePath).Returns(expectedSize);

        // When
        var result = getFileSizeUseCase.Execute(filePath);

        // Then
        result.Should().Be(expectedSize);
        fileService.Received(1).GetFileSize(filePath);
    }

    [Test]
    public void Execute_WhenRepositoryThrowsException_ThrowsException()
    {
        // Given
        const string filePath = "invalidFilePath.txt";
        fileService.GetFileSize(filePath).Throws(new FileNotFoundException("File not found"));

        // When
        Action act = () => getFileSizeUseCase.Execute(filePath);

        // Then
        act.Should().Throw<FileNotFoundException>().WithMessage("File not found");
        fileService.Received(1).GetFileSize(filePath);
    }
}
