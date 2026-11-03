namespace notif_be;

public class AlertRule
{
    public AlertRule(string[] eventTypes, bool emailEnabled, bool slackEnabled)
    {
        if (eventTypes.Length == 0)
        {
            throw new ArgumentException("At least one event type is required.", nameof(eventTypes));
        }
    }
}
