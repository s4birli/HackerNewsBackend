using Microsoft.AspNetCore.Mvc;
using HackerNewsBackend.Services;
using HackerNewsBackend.Models;

namespace HackerNewsBackend.Controllers
{
    public class StoriesResponse
    {
        public List<Story> Stories { get; set; }
        public int Total { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public StoriesController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetStoriesPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (stories, total) = await _hackerNewsService.GetStoriesPageAsync(page, pageSize);
            return Ok(new StoriesResponse { Stories = stories, Total = total });
        }
    }
}
