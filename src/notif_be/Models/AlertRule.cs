namespace notif_be.Models;

public class AlertRule : IModel
{
    private readonly string[] eventTypes;

    public int Id { get; set; }
    public bool EmailEnabled { get; }
    public bool SlackEnabled { get; }

    public bool Matches(string eventType) => eventTypes.Contains(eventType);

    public AlertRule(int id, string[] eventTypes, bool emailEnabled, bool slackEnabled)
    {
        if (!emailEnabled && !slackEnabled)
        {
            throw new ArgumentException("At least one notification channel is required.");
        }

        if (eventTypes.Length == 0)
        {
            throw new ArgumentException("At least one event type is required.", nameof(eventTypes));
        }

        foreach (var eventType in eventTypes)
        {
            if (eventType is not ("breaking-news" or "market-movement" or "natural-disaster"))
            {
                throw new ArgumentException("Unknown event type.", nameof(eventTypes));
            }
        }

        this.eventTypes = (string[])eventTypes.Clone();
        Id = id;
        EmailEnabled = emailEnabled;
        SlackEnabled = slackEnabled;
    }
}
