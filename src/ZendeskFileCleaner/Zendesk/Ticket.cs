using System.Text.Json.Serialization;

namespace ZendeskFileCleaner.Zendesk;

public sealed class Ticket : ITicket
{
    [JsonPropertyName("id")]
    public long Id { get; init; }
    
    [JsonPropertyName("assignee_id")]
    public long? AssigneeId { get; init; }
    
    [JsonPropertyName("status")]
    public required string Status { get; init; }
    
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; init; }
}