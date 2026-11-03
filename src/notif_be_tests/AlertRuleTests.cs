using notif_be;

namespace notif_be_tests;

public class AlertRuleTests
{
    [Fact]
    public void Alert_rule_retains_its_supplied_identifier()
    {
        var rule = new AlertRule(
            id: 17,
            eventTypes: new[] { "breaking-news" },
            emailEnabled: true,
            slackEnabled: false);

        Assert.Equal(17, rule.Id);
    }

    [Fact]
    public void Alert_rule_retains_both_notification_channels_enabled()
    {
        var rule = new AlertRule(
            id: 17,
            eventTypes: new[] { "breaking-news" },
            emailEnabled: true,
            slackEnabled: true);

        Assert.Equal((true, true), (rule.EmailEnabled, rule.SlackEnabled));
    }

    [Theory]
    [InlineData("breaking-news", true)]
    [InlineData("natural-disaster", true)]
    [InlineData("market-movement", false)]
    public void Alert_rule_matches_any_selected_event_type(string eventType, bool expectedMatch)
    {
        var rule = new AlertRule(
            id: 17,
            eventTypes: new[] { "breaking-news", "natural-disaster" },
            emailEnabled: true,
            slackEnabled: false);

        Assert.Equal(expectedMatch, rule.Matches(eventType));
    }

    [Fact]
    public void Alert_rule_rejects_both_notification_channels_disabled()
    {
        Assert.Throws<ArgumentException>(() => new AlertRule(
            id: 17,
            eventTypes: new[] { "breaking-news" },
            emailEnabled: false,
            slackEnabled: false));
    }

    [Theory]
    [InlineData("unknown-event")]
    [InlineData("breaking-news", "unknown-event")]
    public void Alert_rule_rejects_an_unknown_event_type(params string[] eventTypes)
    {
        Assert.Throws<ArgumentException>(() => new AlertRule(
            id: 17,
            eventTypes: eventTypes,
            emailEnabled: true,
            slackEnabled: false));
    }

    [Fact]
    public void Alert_rule_rejects_an_empty_event_type_selection()
    {
        Assert.Throws<ArgumentException>(() => new AlertRule(
            id: 17,
            eventTypes: Array.Empty<string>(),
            emailEnabled: true,
            slackEnabled: false));
    }
}
