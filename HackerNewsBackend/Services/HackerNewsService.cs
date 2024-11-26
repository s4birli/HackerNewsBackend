using System.Text.Json;
using HackerNewsBackend.Models;
using HackerNewsBackend.Services;
using Microsoft.Extensions.Caching.Memory;

public class HackerNewsService : IHackerNewsService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    private const string CacheKey_NewStories = "NewStoriesCache";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

    public HackerNewsService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<List<int>> GetNewStoryIdsAsync()
    {
        if (_cache.TryGetValue(CacheKey_NewStories, out List<int> cachedStoryIds))
        {
            return cachedStoryIds;
        }

        // Ensure `BaseAddress` is properly appended
        var response = await _httpClient.GetAsync("newstories.json");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var storyIds = JsonSerializer.Deserialize<List<int>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        _cache.Set(CacheKey_NewStories, storyIds, CacheExpiration);

        return storyIds;
    }

    public async Task<Story> GetStoryAsync(int id)
    {
        var response = await _httpClient.GetAsync($"item/{id}.json");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<Story>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<(List<Story> Stories, int Total)> GetStoriesPageAsync(int page, int pageSize)
    {
        var storyIds = await GetNewStoryIdsAsync();
        int total = storyIds.Count;

        var pageIds = storyIds.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var tasks = pageIds.Select(id => GetStoryAsync(id)).ToList();
        var stories = (await Task.WhenAll(tasks)).Where(story => story != null).ToList();

        return (stories, total);
    }
}
