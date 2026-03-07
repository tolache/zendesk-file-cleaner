using System.Text.Json.Serialization;

namespace ZendeskFileCleaner.Zendesk;

public sealed class TicketsResponse
{
    [JsonPropertyName("tickets")]
    public List<Ticket> Tickets { get; init; } = [];
}