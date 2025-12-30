using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Octokit;
using GitHubPortfolio.Services.Models;

namespace GitHubPortfolio.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubOptions _options;
        private readonly IMemoryCache _cache;

        private const string PortfolioCacheKey = "PortfolioCache";
        private const string LastUpdateCacheKey = "LastUpdate";

        public GitHubService(IOptions<GitHubOptions> options, IMemoryCache cache)
        {
            _options = options.Value;
            _cache = cache;

            // יצירת הקליינט והזדהות מול GitHub באמצעות Token שנלקח מה-User Secrets
            _client = new GitHubClient(new ProductHeaderValue("MyPortfolioApp"))
            {
                Credentials = new Credentials(_options.Token)
            };
        }

        public async Task<IEnumerable<RepositoryDto>> GetPortfolio()
        {
            // שליפת האירוע האחרון של המשתמש ב-GitHub כדי לבדוק אם חל שינוי
            var lastEvents = await _client.Activity.Events.GetAllUserPerformed(_options.UserName);
            var latestEventTime = lastEvents.FirstOrDefault()?.CreatedAt;

            // אם המידע קיים ב-Cache,  נבדוק אם זמן העדכון בGitHub מאוחר יותר מזמן השמירה בזיכרון
            if (_cache.TryGetValue(PortfolioCacheKey, out IEnumerable<RepositoryDto> cachedPortfolio))
            {
                _cache.TryGetValue(LastUpdateCacheKey, out DateTimeOffset? lastCacheUpdate);
                if (latestEventTime <= lastCacheUpdate)
                {
                    return cachedPortfolio; // החזרה מהירה מהזיכרון במידה ולא חל שינוי
                }
            }

            // --- שליפת נתונים מורחבת ---
            var repos = await _client.Repository.GetAllForUser(_options.UserName);
            var portfolio = new List<RepositoryDto>();

            foreach (var repo in repos)
            {
                // שליפת מידע נוסף שלא מגיע באובייקט ה-Repository הבסיסי
                var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);
                var prs = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);

                portfolio.Add(new RepositoryDto
                {
                    Name = repo.Name,
                    Description = repo.Description,
                    HtmlUrl = repo.HtmlUrl,
                    Stars = repo.StargazersCount,
                    LastCommit = repo.PushedAt,
                    Languages = languages.Select(l => l.Name).ToList(),
                    PullRequestsCount = prs.Count
                });
            }

            // שמירה ב-Cache ועדכון זמן הבדיקה האחרון
            _cache.Set(PortfolioCacheKey, portfolio, TimeSpan.FromMinutes(10));
            _cache.Set(LastUpdateCacheKey, latestEventTime);

            return portfolio;
        }

        public async Task<IReadOnlyList<Repository>> SearchRepositories(string? repoName, string? language, string? user)
        {
            // מימוש חיפוש כללי GitHub עם סינונים אופציונליים
            var request = new SearchRepositoriesRequest(repoName);
            if (!string.IsNullOrEmpty(language)) request.Language = Enum.Parse<Language>(language, true);
            if (!string.IsNullOrEmpty(user)) request.User = user;

            var result = await _client.Search.SearchRepo(request);
            return result.Items;
        }
    }
}