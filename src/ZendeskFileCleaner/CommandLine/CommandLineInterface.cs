using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ZendeskFileCleaner.CommandLine;

public class CommandLineInterface(IServiceProvider serviceProvider)
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
        Option<long> zdUserIdOption = new("--zd-user-id", "-u")
        {
            Required    = true,
            Description = "Zendesk API token."
        };
        Option<bool> dryRunOption = new("--dry-run", "-n")
        {
            Description = "Print what would be deleted, but do not delete anything."
        };
        Option<bool> verboseOption = new("--verbose", "-v")
        {
            Description = "Show debug messages."
        };
        RootCommand rootCommand = new("Removes old Zendesk ticket directories from the disk.")
        {
            pathArg,
            subdomainOption,
            emailOption,
            tokenOption,
            zdUserIdOption,
            dryRunOption,
            verboseOption
        };

        rootCommand.SetAction(async parseResult =>
        {
            ApplicationOptions options = new()
            {
                RootDir   = parseResult.GetValue(pathArg)!,
                Subdomain = parseResult.GetValue(subdomainOption)!,
                Email     = parseResult.GetValue(emailOption)!,
                Token     = parseResult.GetValue(tokenOption)!,
                ZdUserId  = parseResult.GetValue(zdUserIdOption),
                IsDryRun  = parseResult.GetValue(dryRunOption),
                MinLevel  = parseResult.GetValue(verboseOption) ? LogLevel.Debug : LogLevel.Information
            };
            
            IServiceScope scope = serviceProvider.CreateScope();
            ITicketDirectoryCleaner cleaner = scope.ServiceProvider.GetRequiredService<ITicketDirectoryCleaner>();
            
            return await cleaner.CleanTicketDirectories(options);
        });
        
        return rootCommand;
    }
}