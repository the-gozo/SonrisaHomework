using notif_be;
using notif_be.Models;

namespace notif_be_tests;

public class UserTests
{
    [Fact]
    public void User_retains_its_optional_alert_rule()
    {
        var rule = new AlertRule(
            id: 17,
            eventTypes: new[] { "breaking-news" },
            emailEnabled: true,
            slackEnabled: false);
        var user = new User(id: 42, email: "alice@example.com", alertRule: rule);

        Assert.Same(rule, user.AlertRule);
    }

    [Fact]
    public void User_retains_identity_and_email_without_an_alert_rule()
    {
        var user = new User(id: 42, email: "alice@example.com");

        Assert.Equal((42, "alice@example.com", (AlertRule?)null),
            (user.Id, user.Email, user.AlertRule));
    }
}
