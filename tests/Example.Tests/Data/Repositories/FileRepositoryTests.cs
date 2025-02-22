using System.IO.Abstractions.TestingHelpers;
using Example.Data.Repositories;
using FluentAssertions;
using NUnit.Framework;

namespace Example.Tests.Data.Repositories;

[TestFixture]
internal sealed class FileRepositoryTests
{
    private MockFileSystem mockFileSystem = null!;
    private FileRepository fileRepository = null!;

    [SetUp]
    public void SetUp()
    {
        mockFileSystem = new MockFileSystem();
        fileRepository = new FileRepository(mockFileSystem);
    }

    [Test]
    public void GetFileSize_WhenFileExists_ShouldReturnFileSize()
    {
        // Given
        const string filePath = @"C:\test\file.txt";
        const string fileContent = "Hello, world!";
        var expectedFileSize = fileContent.Length;

        mockFileSystem.AddFile(filePath, new MockFileData(fileContent));

        // When
        var result = fileRepository.GetFileSize(filePath);

        // Then
        result.Should().Be(expectedFileSize);
    }

    [Test]
    public void GetFileSize_WhenFileDoesNotExist_ShouldThrowFileNotFoundException()
    {
        // Given
        const string filePath = @"C:\test\nonexistent.txt";

        // When, Then
        fileRepository.Invoking(repo => repo.GetFileSize(filePath)).Should().Throw<FileNotFoundException>();
    }
}
