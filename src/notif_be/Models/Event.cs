namespace notif_be.Models;

public class Event : IModel
{
    public int Id { get; set; }
    public string EventType { get; }
    public string Title { get; }
    public string Body { get; }

    public Event(int id, string eventType, string title, string body)
    {
        if (eventType is not ("breaking-news" or "market-movement" or "natural-disaster"))
        {
            throw new ArgumentException("Unknown event type.", nameof(eventType));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("An event title is required.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException("An event body is required.", nameof(body));
        }

        Id = id;
        EventType = eventType;
        Title = title;
        Body = body;
    }
}
