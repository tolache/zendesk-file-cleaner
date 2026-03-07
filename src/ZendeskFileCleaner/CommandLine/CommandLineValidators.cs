using System.CommandLine.Parsing;
using System.ComponentModel.DataAnnotations;

namespace ZendeskFileCleaner.CommandLine;

public static class CommandLineValidators
{
    public static void ValidatePath(ArgumentResult result)
    {
        DirectoryInfo path = result.GetValueOrDefault<DirectoryInfo>();
        if (!path.Exists)
        {
            result.AddError($"Directory '{path}' doesn't exist. Specify a valid path.");
        }
    }

    public static  void ValidateSubdomain(OptionResult result)
    {
        string value = result.GetValueOrDefault<string>();
        if (string.IsNullOrWhiteSpace(value))
        {
            result.AddError("`--subdomain` cannot be empty or whitespace.");
            return;
        }

        foreach (char c in value)
        {
            if (char.IsLetterOrDigit(c) || c == '-' || c == '.') continue;
            result.AddError("`--subdomain` may only contain letters, digits, hyphens, and dots.");
            return;
        }
    }

    public static void ValidateEmail(OptionResult result)
    {
        string value = result.GetValueOrDefault<string>();
        if (string.IsNullOrWhiteSpace(value))
        {
            result.AddError("`--email` cannot be empty or whitespace.");
        }
        EmailAddressAttribute validator = new();
        if (!validator.IsValid(value))
        {
            result.AddError($"'{value}' is not a valid email address.");
        }
    }
    
    public static void ValidateToken(OptionResult result)
    {
        string value = result.GetValueOrDefault<string>();
        if (string.IsNullOrWhiteSpace(value))
        {
            result.AddError("`--token` cannot be empty or whitespace.");
        }
        
        if (!value.All(c => c <= 0x7F))
        {
            result.AddError("`--token` must contain only ASCII characters.");
        }
    }
}