using notif_be.Models;
using notif_be.Services;

namespace notif_be_tests;

public class NotifyServiceTests
{
    [Fact]
    public async Task Notifying_an_email_enabled_user_requests_email_delivery()
    {
        var emailService = new RecordingEmailService();
        INotifyService notifyService = new NotifyService(emailService);
        var rule = new AlertRule(17, new[] { "breaking-news" }, true, false);
        var user = new User(42, "alice@example.com", rule);
        var worldEvent = new Event(7, "breaking-news", "Important news", "Full report");

        await notifyService.NotifyAsync(user, worldEvent);

        var request = Assert.Single(emailService.Requests);
        Assert.Equal(("alice@example.com", "Important news", "Full report"), request);
    }

    private sealed class RecordingEmailService : IEmailService
    {
        public List<(string RecipientEmail, string Subject, string Body)> Requests { get; } = new();

        public Task SendAsync(string recipientEmail, string subject, string body)
        {
            throw new NotImplementedException();
        }
    }
}
