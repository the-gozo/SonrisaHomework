using notif_be;

namespace notif_be_tests;

public class AlertRuleTests
{
    [Theory]
    [InlineData("unknown-event")]
    [InlineData("breaking-news", "unknown-event")]
    public void Alert_rule_rejects_an_unknown_event_type(params string[] eventTypes)
    {
        Assert.Throws<ArgumentException>(() => new AlertRule(
            eventTypes: eventTypes,
            emailEnabled: true,
            slackEnabled: false));
    }

    [Fact]
    public void Alert_rule_rejects_an_empty_event_type_selection()
    {
        Assert.Throws<ArgumentException>(() => new AlertRule(
            eventTypes: Array.Empty<string>(),
            emailEnabled: true,
            slackEnabled: false));
    }
}
