GitHub Portfolio API
This project is a sophisticated portfolio management system that integrates with the GitHub API.
It demonstrates advanced backend capabilities, focusing on data mapping, API optimization, and a
custom-built smart caching mechanism.

Core Features
- Detailed Portfolio Fetching: Automatically retrieves personal repositories including data that
isn't available in basic requests, such as programming languages and Pull Request counts.
- Smart Caching (The Challenge): Implemented a high-performance In-Memory Cache that
reduces API latency. The system is "event-aware"—it checks GitHub's activity stream and only
refreshes the cache if a new event (push, commit, etc.) is detected.
- Global Repository Search: A public endpoint to search any repository across GitHub by name,
language, or user without requiring authentication.
- Secure Configuration: Fully implemented the Options Pattern with User Secrets to ensure
sensitive credentials like API tokens are never hardcoded or exposed.

Tech Stack
- Backend: .NET 8 Web API, C#.
- API Client: Octokit (Official GitHub client for .NET).
- Caching: Microsoft.Extensions.Caching.Memory.
- Architecture: Clean Architecture with separation between the API Layer and the Service Layer.

How it Works
1. Authentication: The service authenticates with GitHub using a Personal Access Token stored
   securely in the environment's secrets.
2. Data Transformation (DTO): Raw data from GitHub is transformed into a specialized
   RepositoryDto, providing a clean and relevant JSON response to the client.
3. Cache Validation: * When a request comes in, the system fetches the user's latest GitHub events.
   - If the latestEventTime is newer than the lastCacheUpdate, the system performs a full fetch.
   - Otherwise, it serves the data instantly from the memory cache, drastically improving response times.
  
Setup
To run this project locally:
1. Clone the repository.
2. Add your GitHub credentials to your User Secrets:
   {
  "GitHubSettings": {
    "Token": "YOUR_TOKEN_HERE",
    "UserName": "YOUR_GITHUB_USERNAME"
  }
}
3. Launch the application and use the Swagger UI to interact with the endpoints.
