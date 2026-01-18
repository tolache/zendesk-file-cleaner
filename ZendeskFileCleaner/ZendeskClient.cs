namespace ZendeskFileCleaner;

using System.Net.Http;
using System.Net.Http.Headers;

public class ZendeskClient
{
    private readonly HttpClient _zendeskClient;
    
    public ZendeskClient(string subdomain, string apiKey)
    {
        _zendeskClient = new HttpClient();
        _zendeskClient.DefaultRequestHeaders.Accept.Clear();
        _zendeskClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _zendeskClient.DefaultRequestHeaders.Add("User-Agent", "ZendeskFileCleaner");
    }
    
    public static async Task FetchTicketInfoAsync()
    {
        
    }
}