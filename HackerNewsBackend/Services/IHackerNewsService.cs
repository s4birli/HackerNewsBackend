using HackerNewsBackend.Models;

namespace HackerNewsBackend.Services
{
    public interface IHackerNewsService
    {
        Task<List<int>> GetNewStoryIdsAsync();

        Task<Story> GetStoryAsync(int id);

        Task<(List<Story> Stories, int Total)> GetStoriesPageAsync(int page, int pageSize);
    }
}

