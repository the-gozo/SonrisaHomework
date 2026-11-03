namespace notif_be;

public class AlertRule
{
    private readonly string[] eventTypes;

    public bool EmailEnabled { get; }
    public bool SlackEnabled { get; }

    public bool Matches(string eventType) => eventTypes.Contains(eventType);

    public AlertRule(string[] eventTypes, bool emailEnabled, bool slackEnabled)
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
        EmailEnabled = emailEnabled;
        SlackEnabled = slackEnabled;
    }
}
