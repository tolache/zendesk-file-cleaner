using System.CommandLine;
using ZendeskFileCleaner;

Argument<DirectoryInfo> pathArg = new("path")
{
    Arity       = ArgumentArity.ExactlyOne,
    Description = "The path of the parent directory containing Zendesk ticket directories that need to be cleaned.",
    Validators  = { CommandLineValidators.ValidatePath }
};
Option<string> subdomainOption = new("--subdomain", "-s")
{
    Required    = true,
    Description = "Zendesk subdomain (<subdomain>.zendesk.com).",
    Validators  = { CommandLineValidators.ValidateSubdomain }
};
Option<string> apiKeyOption = new("--api-key", "-a")
{
    Required    = true,
    Description = "Zendesk API token.",
    Validators  = { CommandLineValidators.ValidateApiKey }
};
Option<bool> dryRunOption = new("--dry-run", "-n")
{
    Description = "Print what would be deleted, but do not delete anything."
};
RootCommand rootCommand = new("Removes old Zendesk ticket directories from the disk.")
{
    pathArg,
    subdomainOption,
    apiKeyOption,
    dryRunOption,
};

rootCommand.SetAction(parseResult =>
{
    DirectoryInfo path = parseResult.GetValue(pathArg)!;
    string subdomain   = parseResult.GetValue(subdomainOption)!;
    string apiKey      = parseResult.GetValue(apiKeyOption)!;
    bool dryRun        = parseResult.GetValue(dryRunOption);

    return TicketDirectoryProcessor.ProcessTicketDirectories(path, subdomain, apiKey, dryRun);
});

return await rootCommand.Parse(args).InvokeAsync();