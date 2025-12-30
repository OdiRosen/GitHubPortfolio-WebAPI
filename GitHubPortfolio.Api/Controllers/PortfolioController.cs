using GitHubPortfolio.Services;
using GitHubPortfolio.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace GitHubPortfolio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public PortfolioController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService; // הזרקת התלות של ה-Service
        }

        // שליפת הפורטפוליו האישי מפורט ומאובטח (מבוסס Cache)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepositoryDto>>> Get()
        {
            var repos = await _gitHubService.GetPortfolio();
            return Ok(repos);
        }

        // חיפוש כללי ברחבי GitHub ללא צורך בטוקן
        [HttpGet("search")]
        public async Task<ActionResult<IReadOnlyList<Repository>>> Search(
            [FromQuery] string? repoName,
            [FromQuery] string? language,
            [FromQuery] string? user)
        {
            var results = await _gitHubService.SearchRepositories(repoName, language, user);
            return Ok(results);
        }
    }
}