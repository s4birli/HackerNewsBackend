using HackerNewsBackend.Models;

namespace HackerNewsBackend.Tests.MockData
{
    public static class SampleStories
    {
        public static List<Story> GetSampleStories()
        {
            return new List<Story>
            {
                new Story
                {
                    Id = 1,
                    Title = "Sample Story 1",
                    By = "Author 1",
                    Time = 1732507598,
                    Url = "https://example.com/story1",
                    Score = 100,
                    Descendants = 5,
                    Type = "story"
                },
                new Story
                {
                    Id = 2,
                    Title = "Sample Story 2",
                    By = "Author 2",
                    Time = 1732507599,
                    Url = "https://example.com/story2",
                    Score = 50,
                    Descendants = 10,
                    Type = "story"
                }
            };
        }
    }
}
