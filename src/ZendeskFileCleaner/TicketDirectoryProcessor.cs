using ZendeskFileCleaner.Zendesk;

namespace ZendeskFileCleaner;

public class TicketDirectoryProcessor : ITicketDirectoryProcessor
{
    public async Task<int> ProcessTicketDirectories(
        DirectoryInfo ticketParentDir,
        string subdomain,
        string email,
        string token,
        bool dryRun,
        IZendeskClient zendeskClient)
    {
        IEnumerable<ITicket> tickets = await zendeskClient.FetchTicketsAsync(GetTicketIds(ticketParentDir), subdomain, email, token);
        foreach (ITicket ticket in tickets)
        {
            Console.WriteLine("Id: " + ticket.Id + Environment.NewLine +
                              "Assignee: " + ticket.AssigneeId + Environment.NewLine +
                              "Status: " + ticket.Status + Environment.NewLine +
                              "Update at: " + ticket.UpdatedAt + Environment.NewLine);
        }

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

public interface ITicketDirectoryProcessor
{
    public Task<int> ProcessTicketDirectories(
        DirectoryInfo ticketParentDir, 
        string subdomain,
        string email,
        string token,
        bool dryRun, 
        IZendeskClient zendeskClient);
}