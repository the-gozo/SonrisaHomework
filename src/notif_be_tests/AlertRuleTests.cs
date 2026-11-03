using notif_be;

namespace notif_be_tests;

public class AlertRuleTests
{
    [Fact]
    public void Alert_rule_rejects_an_empty_event_type_selection()
    {
        Assert.Throws<ArgumentException>(() => new AlertRule(
            eventTypes: Array.Empty<string>(),
            emailEnabled: true,
            slackEnabled: false));
    }
}
