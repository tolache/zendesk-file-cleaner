namespace ZendeskFileCleaner;

public static class TicketDirectoryProcessor
{
    public static async Task<int> ProcessTicketDirectories(DirectoryInfo ticketParentDir, string subdomain, string apiKey, bool dryRun)
    {
        await ZendeskClient.FetchTicketInfoAsync(GetTicketIds(ticketParentDir), subdomain, apiKey);

        return 0;
    }

    private static long[] GetTicketIds(DirectoryInfo ticketParentDir)
    {
        List<long> ticketIds = [];
        Console.WriteLine($"Found the following directories with numerical names inside {ticketParentDir.FullName}:");
        foreach (DirectoryInfo subDirectoryInfo in ticketParentDir.EnumerateDirectories())
        {
            if (long.TryParse(subDirectoryInfo.Name, out long ticketId))
            {
                ticketIds.Add(ticketId);
                Console.Write($"{subDirectoryInfo.Name} ");
            }
        }
        Console.WriteLine(Environment.NewLine + $"Total of {ticketIds.Count} directories.");
        return ticketIds.ToArray();
    }
}