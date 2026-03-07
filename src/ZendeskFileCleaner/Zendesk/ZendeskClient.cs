using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ZendeskFileCleaner.Zendesk;

public class ZendeskClient : IZendeskClient
{
    private const int ZendeskPerRequestTicketLimit = 100;
    
    public async Task<IEnumerable<ITicket>> FetchTicketsAsync(long[] ticketIds, string subdomain, string email, string token)
    {
        string credentials = $"{email}/token:{token}";
        string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
        
        using HttpClient zendeskClient = new();
        zendeskClient.DefaultRequestHeaders.Accept.Clear();
        zendeskClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
        zendeskClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        zendeskClient.DefaultRequestHeaders.Add("User-Agent", "ZendeskFileCleaner");
        
        List<ITicket> result = [];
        int chunkCount = (ticketIds.Length + ZendeskPerRequestTicketLimit - 1) / ZendeskPerRequestTicketLimit;
        for (int i = 0; i < chunkCount; i++)
        {
            long[] currentChunk = ticketIds.Skip(ZendeskPerRequestTicketLimit * i).Take(ZendeskPerRequestTicketLimit).ToArray();
            string currentChunkString = string.Join(",", currentChunk);
            string uri = $"https://{subdomain}.zendesk.com/api/v2/tickets/show_many?ids={currentChunkString}";
            Console.WriteLine($"Retrieving ticket info from Zendesk, chunk {i + 1} of {chunkCount}.");
            string json = await zendeskClient.GetStringAsync(uri);
            TicketsResponse? response = JsonSerializer.Deserialize<TicketsResponse>(json);
            result.AddRange(response?.Tickets ?? Enumerable.Empty<ITicket>());
        }

        return result;
    }
}

public interface IZendeskClient
{
    public Task<IEnumerable<ITicket>> FetchTicketsAsync(long[] ticketIds, string subdomain, string email, string token);
}