using System.CommandLine;
using System.CommandLine.Parsing;

Option<DirectoryInfo> pathOption = new("--path")
{
    Required = true,
    Description = "The path of the parent directory containing Zendesk ticket directories that need to be cleaned."
};
Option<string> subdomainOption = new("--subdomain", "-s")
{
    Required = true,
    Description = "Zendesk subdomain (e.g., if your Zendesk domain is 'example.zendesk.com', specify 'example')."
};
Option<string> apiKeyOption = new("--api-key", "-a")
{
    Required = true,
    Description = "Zendesk API token."
};
Option<bool> dryRunOption = new("--dry-run", "-n")
{
    Description = "Print what would be deleted, but do not delete anything."
};
Option<bool> verboseOption = new("--verbose", "-v")
{
    Description = "Shows verbose output during execution."
};
Option<bool> versionOption = new("--version", "-V")
{
    Description = "Displays the version information."
};
RootCommand rootCommand = new("Removes old Zendesk ticket directories from the disk.")
{
    pathOption,
    subdomainOption,
    apiKeyOption,
    dryRunOption,
    verboseOption,
    versionOption,
};

rootCommand.SetAction(parseResult =>
{
    if (parseResult.Errors.Count == 0 && 
        parseResult.GetValue(pathOption) is { } parsedPath &&
        parseResult.GetValue(subdomainOption) is { } subdomain)
    {
        Console.WriteLine($"Found the following directories inside {parsedPath.FullName}:");
        foreach (DirectoryInfo subDirectoryInfo in parsedPath.EnumerateDirectories())
        {
            Console.Write($"{subDirectoryInfo.Name} ");
        }

        Console.WriteLine();
        return 0;
    }

    foreach (ParseError error in parseResult.Errors)
    {
        Console.Error.WriteLine(error.Message);
        return 1;
    }

    return 0;
});

return await rootCommand.Parse(args).InvokeAsync();