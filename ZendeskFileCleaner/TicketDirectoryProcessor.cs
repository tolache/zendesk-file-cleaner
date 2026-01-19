namespace ZendeskFileCleaner;

public static class TicketDirectoryProcessor
{
    public static async Task<int> ProcessTicketDirectories(DirectoryInfo ticketParentDir, string subdomain, string apiKey)
    {
        using ZendeskClient zendeskClient = new(subdomain, apiKey);
        await zendeskClient.FetchTicketInfoAsync(GetTicketIds(ticketParentDir));
        return 0;
    }

    // Gets the ticket directory names from the specified parent directory and returns them as an array of integers 
    private static int[] GetTicketIds(DirectoryInfo ticketParentDir)
    {
        throw new NotImplementedException();
    }
}