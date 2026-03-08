using Microsoft.Extensions.Logging;

namespace ZendeskFileCleaner;

public class ApplicationOptions
{
    public required DirectoryInfo RootDir { get; init; }
    public required string Subdomain { get; init; }
    public required string Email { get; init; }
    public required string Token { get; init; }
    public required long ZdUserId { get; init; }
    public bool IsDryRun { get; init; }
    public LogLevel MinLevel { get; init; }
}