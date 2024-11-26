using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;

namespace HackerNewsBackend.Tests
{
    public class HackerNewsServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly HackerNewsService _hackerNewsService;

        public HackerNewsServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/")
            };

            _memoryCacheMock = new Mock<IMemoryCache>();
            _hackerNewsService = new HackerNewsService(_httpClient, _memoryCacheMock.Object);
        }

        [Fact]
        public async Task GetNewStoryIdsAsync_ShouldReturnListOfIds()
        {
            // Arrange
            var fakeResponse = JsonSerializer.Serialize(new List<int> { 1, 2, 3 });
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fakeResponse)
                });

            object cacheEntry = null;
            _memoryCacheMock
                .Setup(mc => mc.TryGetValue(It.IsAny<object>(), out cacheEntry))
                .Returns(false); 

            _memoryCacheMock
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>()); 

            // Act
            var result = await _hackerNewsService.GetNewStoryIdsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(new List<int> { 1, 2, 3 }, result);
        }
    }
}
