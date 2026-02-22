using System.CommandLine.Parsing;

namespace ZendeskFileCleaner;

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

    public static  void ValidateApiKey(OptionResult result)
    {
        string value = result.GetValueOrDefault<string>();
        if (string.IsNullOrWhiteSpace(value))
        {
            result.AddError("`--api-key` cannot be empty or whitespace.");
        }
    }
}