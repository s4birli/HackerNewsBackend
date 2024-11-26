using HackerNewsBackend.Tests.MockData;
using Moq;
using Microsoft.AspNetCore.Mvc;
using HackerNewsBackend.Services;
using HackerNewsBackend.Controllers;

public class StoriesControllerTests
{
    [Fact]
    public async Task GetStoriesPage_ReturnsMockedStories()
    {
        // Arrange
        var mockStories = SampleStories.GetSampleStories();
        var mockService = new Mock<IHackerNewsService>();

        mockService
            .Setup(service => service.GetStoriesPageAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((mockStories, mockStories.Count));

        var controller = new StoriesController(mockService.Object);

        // Act
        var result = await controller.GetStoriesPage(1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<StoriesResponse>(okResult.Value);

        Assert.NotNull(response);
        Assert.Equal(mockStories.Count, response.Total);
        Assert.Equal(mockStories[0].Title, response.Stories[0].Title);
    }

}
