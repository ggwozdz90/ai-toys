using Example.Adapter.Adapters;
using Example.Application.UseCases;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Example.Tests.Adapter.Adapters;

[TestFixture]
internal sealed class FileAdapterTests
{
    private IGetFileSizeUseCase getFileSizeUseCase = null!;
    private FileAdapter fileAdapter = null!;

    [SetUp]
    public void Setup()
    {
        getFileSizeUseCase = Substitute.For<IGetFileSizeUseCase>();
        fileAdapter = new FileAdapter(getFileSizeUseCase);
    }

    [Test]
    public void GetFileSize_WithValidFilePath_ReturnsFileSize()
    {
        // Given
        const string filePath = "validFilePath.txt";
        const long expectedSize = 1024L;
        getFileSizeUseCase.Execute(filePath).Returns(expectedSize);

        // When
        var result = fileAdapter.GetFileSize(filePath);

        // Then
        result.Should().Be(expectedSize);
        getFileSizeUseCase.Received(1).Execute(filePath);
    }

    [Test]
    public void GetFileSize_WhenUseCaseThrowsException_ThrowsException()
    {
        // Given
        const string filePath = "invalidFilePath.txt";
        getFileSizeUseCase.Execute(filePath).Throws(new FileNotFoundException("File not found"));

        // When
        Action act = () => fileAdapter.GetFileSize(filePath);

        // Then
        act.Should().Throw<Exception>().WithMessage("File not found");
        getFileSizeUseCase.Received(1).Execute(filePath);
    }
}
