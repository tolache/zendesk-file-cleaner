namespace ZendeskFileCleaner;

using System.Net.Http;
using System.Net.Http.Headers;

public class ZendeskClient : IZendeskClient
{
    private const int ZendeskPerRequestTicketLimit = 100;
    
    public async Task<long[]> FetchTicketInfoAsync(long[] ticketIds, string subdomain, string apiKey)
    {
        using HttpClient zendeskClient = new();
        zendeskClient.DefaultRequestHeaders.Accept.Clear();
        zendeskClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        zendeskClient.DefaultRequestHeaders.Add("User-Agent", "ZendeskFileCleaner");
        
        long[] result = [];
        int chunkCount = (ticketIds.Length + ZendeskPerRequestTicketLimit - 1) / ZendeskPerRequestTicketLimit;
        for (int i = 0; i < chunkCount; i++)
        {
            long[] currentChunk = ticketIds.Skip(ZendeskPerRequestTicketLimit * i).Take(ZendeskPerRequestTicketLimit).ToArray();
            string currentChunkString = string.Join(",", currentChunk);
            string uri = $"https://{subdomain}.zendesk.com/api/v2/tickets/show_many.json?ids={currentChunkString}";
            Console.WriteLine($"Retrieving ticket info from Zendesk, chunk {i + 1} of {chunkCount}.");
            // var json = await zendeskClient.GetStringAsync($"https://{subdomain}.zendesk.com/api/v2/tickets/show_many.json?ids=")
        }

        return result;
    }
}

public interface IZendeskClient
{
    public Task<long[]> FetchTicketInfoAsync(long[] ticketIds, string subdomain, string apiKey);
}