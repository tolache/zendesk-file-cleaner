namespace ZendeskFileCleaner;

public class TicketDirectoryProcessor : ITicketDirectoryProcessor
{
    public async Task<int> ProcessTicketDirectories(
        DirectoryInfo ticketParentDir,
        string subdomain,
        string apiKey,
        bool dryRun,
        IZendeskClient zendeskClient)
    {
        await zendeskClient.FetchTicketInfoAsync(GetTicketIds(ticketParentDir), subdomain, apiKey);

        return 0;
    }

    private long[] GetTicketIds(DirectoryInfo ticketParentDir)
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

public interface ITicketDirectoryProcessor
{
    public Task<int> ProcessTicketDirectories(
        DirectoryInfo ticketParentDir, 
        string subdomain,
        string apiKey,
        bool dryRun, 
        IZendeskClient zendeskClient);
}