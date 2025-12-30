using GitHubPortfolio.Services.Models;
using Octokit;

namespace GitHubPortfolio.Services
{
    // הגדרת ה-Interface מאפשרת הפרדה בין המימוש לבין ה-Controller (Decoupling)
    public interface IGitHubService
    {
        // מחזירה את רשימת הפרויקטים האישית בפורמט DTO הכולל שפות ו-Pull Requests
        Task<IEnumerable<RepositoryDto>> GetPortfolio();

        // מאפשרת חיפוש כללי ב GitHub ללא צורך בהזדהות 
        Task<IReadOnlyList<Repository>> SearchRepositories(string? repoName, string? language, string? user);
    }
}