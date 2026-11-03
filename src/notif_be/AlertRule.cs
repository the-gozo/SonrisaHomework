namespace notif_be;

public class AlertRule
{
    public AlertRule(string[] eventTypes, bool emailEnabled, bool slackEnabled)
    {
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
    }
}
