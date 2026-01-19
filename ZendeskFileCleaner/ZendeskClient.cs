namespace ZendeskFileCleaner;

using System.Net.Http;
using System.Net.Http.Headers;

public class ZendeskClient : IDisposable
{
    private readonly HttpClient _zendeskClient;
    
    public ZendeskClient(string subdomain, string apiKey)
    {
        _zendeskClient = new HttpClient();
        _zendeskClient.DefaultRequestHeaders.Accept.Clear();
        _zendeskClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _zendeskClient.DefaultRequestHeaders.Add("User-Agent", "ZendeskFileCleaner");
    }
    
    // Takes an array of ticket numbers, forms an sends an API request to Zendesk,
    // and returns an array of closed tickets with closure dates 
    public async Task FetchTicketInfoAsync(int[] ticketIds)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _zendeskClient.Dispose();
    }
}