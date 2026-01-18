using System.CommandLine;
using System.CommandLine.Parsing;

Option<bool> verboseOption = new("--verbose", "-v")
{
    Description = "Shows verbose output during execution."
};
Option<bool> versionOption = new("--version", "-V")
{
    Description = "Displays the version information."
};
Option<bool> dryRunOption = new("--dry-run", "-n")
{
    Description = "Print what would be deleted, but do not delete anything."
};
Option<DirectoryInfo> pathOption = new(name: "--path")
{
    Required = true,
    Description = "The path of the parent directory containing Zendesk ticket directories."
};
RootCommand rootCommand = new("Removes old Zendesk ticket directories from the disk.")
{
    verboseOption,
    versionOption,
    versionOption,
    dryRunOption,
    pathOption
};

ParseResult parseResult = rootCommand.Parse(args);
if (parseResult.Errors.Count == 0 && parseResult.GetValue(pathOption) is DirectoryInfo parsedPath)
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