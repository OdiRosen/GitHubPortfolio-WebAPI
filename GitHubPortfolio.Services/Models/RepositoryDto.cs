namespace GitHubPortfolio.Services.Models
{
    public class RepositoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? HtmlUrl { get; set; }
        public List<string> Languages { get; set; } = new();
        public int Stars { get; set; }
        public int PullRequestsCount { get; set; }
        public DateTimeOffset? LastCommit { get; set; }
    }
}