using System.Globalization;
using Microsoft.Extensions.Logging;
using ZendeskFileCleaner.Zendesk;

namespace ZendeskFileCleaner;

public class TicketDirectoryCleaner(IZendeskClient zendeskClient) : ITicketDirectoryCleaner
{
    private const int MaxAgeDays = 60;
    
    public async Task<int> CleanTicketDirectories(ApplicationOptions options)
    {
        int dirsToDelete = 0;
        IEnumerable<ITicket> tickets = await zendeskClient.FetchTicketsAsync(GetTicketIds(options.RootDir), options.Subdomain, options.Email, options.Token);
        DateTime checkTime = DateTime.UtcNow;
        foreach (ITicket ticket in tickets)
        {
            DirectoryInfo ticketDir = new(Path.Combine(options.RootDir.FullName, ticket.Id.ToString()));
            if (options.MinLevel <= LogLevel.Debug)
            {
                Console.WriteLine($"Id: {ticket.Id}{Environment.NewLine}" +
                                  $"Assignee: {ticket.AssigneeId}{Environment.NewLine}" +
                                  $"Status: {ticket.Status}{Environment.NewLine}" +
                                  $"Update at: {ticket.UpdatedAt}{Environment.NewLine}" +
                                  $"Local dir last modified at: {ticketDir.LastWriteTimeUtc}{Environment.NewLine}");
            }

            bool hasDeletableStatus = CheckIsTicketStatusDeletable(ticket, options.ZdUserId);
            bool isOldEnough = CheckIsTicketOldEnough(ticket, ticketDir, checkTime);
            
            if (!(hasDeletableStatus && isOldEnough))
            {
                continue;
            }

            if (options.IsDryRun)
            {
                Console.WriteLine($"Would delete ticket directory: {ticketDir.FullName}");
            }
            else
            {
                Console.WriteLine($"Deleting ticket directory: {ticketDir.FullName}");
                ticketDir.Delete(true);
            }
            dirsToDelete++;
        }

        Console.WriteLine(options.IsDryRun
            ? $"Would delete {dirsToDelete} directories."
            : $"Deleted {dirsToDelete} directories.");

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
    
    private static bool CheckIsTicketStatusDeletable(ITicket ticket, long zdUserId)
    {
        bool isAssignedToCurrentUser = ticket.AssigneeId == zdUserId;
        return isAssignedToCurrentUser
            ? ticket.Status is "closed"
            : ticket.Status is "closed" or "hold";
    }

    private static bool CheckIsTicketOldEnough(ITicket ticket, DirectoryInfo rootDir, DateTime checkTime)
    {
        TimeSpan sinceTicketUpdate = checkTime - ticket.UpdatedAt;
        TimeSpan sinceDirUpdate = checkTime - rootDir.LastWriteTimeUtc;
        return sinceTicketUpdate.TotalDays > MaxAgeDays &&
               sinceDirUpdate.TotalDays > MaxAgeDays;
    }
}

public interface ITicketDirectoryCleaner
{
    public Task<int> CleanTicketDirectories(ApplicationOptions options);
}