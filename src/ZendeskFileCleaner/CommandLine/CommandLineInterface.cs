using System.CommandLine;
using ZendeskFileCleaner.Zendesk;

namespace ZendeskFileCleaner.CommandLine;

public class CommandLineInterface(ITicketDirectoryProcessor processor, IZendeskClient client)
{
    public RootCommand CreateRootCommand()
    {
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
        Option<string> emailOption = new("--email", "-e")
        {
            Required    = true,
            Description = "Zendesk email.",
            Validators  = { CommandLineValidators.ValidateEmail }
        };
        Option<string> tokenOption = new("--token", "-t")
        {
            Required    = true,
            Description = "Zendesk API token.",
            Validators  = { CommandLineValidators.ValidateToken }
        };
        Option<bool> dryRunOption = new("--dry-run", "-n")
        {
            Description = "Print what would be deleted, but do not delete anything."
        };
        RootCommand rootCommand = new("Removes old Zendesk ticket directories from the disk.")
        {
            pathArg,
            subdomainOption,
            emailOption,
            tokenOption,
            dryRunOption,
        };

        rootCommand.SetAction(async parseResult =>
        {
            DirectoryInfo path = parseResult.GetValue(pathArg)!;
            string subdomain   = parseResult.GetValue(subdomainOption)!;
            string email       = parseResult.GetValue(emailOption)!;
            string token       = parseResult.GetValue(tokenOption)!;
            bool dryRun        = parseResult.GetValue(dryRunOption);

            return await processor.ProcessTicketDirectories(path, subdomain, email, token, dryRun, client);
        });
        
        return rootCommand;
    }
}