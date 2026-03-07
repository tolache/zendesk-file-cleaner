namespace ZendeskFileCleaner;

public interface ITicket
{
    long Id { get; }
    long? AssigneeId { get; }
    string Status { get; }
    DateTimeOffset UpdatedAt { get; }
}