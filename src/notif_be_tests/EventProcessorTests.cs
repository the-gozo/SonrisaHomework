using notif_be.Models;
using notif_be.Repositories;
using notif_be.Services;

namespace notif_be_tests;

public class EventProcessorTests
{
    [Fact]
    public async Task Processing_requests_a_notification_only_for_the_matching_user()
    {
        var matchingRule = new AlertRule(17, new[] { "breaking-news" }, true, false);
        var nonmatchingRule = new AlertRule(18, new[] { "market-movement" }, true, false);
        var repository = new InMemoryUserRepository(new[]
        {
            new User(42, "alice@example.com", matchingRule),
            new User(43, "bob@example.com", nonmatchingRule),
            new User(44, "carol@example.com")
        });
        var notifyService = new RecordingNotifyService();
        var processor = new EventProcessor(repository, notifyService);
        var worldEvent = new Event(7, "breaking-news", "Important news", "Full report");

        await processor.ProcessAsync(worldEvent);

        var request = Assert.Single(notifyService.Requests);
        Assert.Equal(42, request.User.Id);
        Assert.Same(worldEvent, request.Event);
    }

    private sealed class RecordingNotifyService : INotifyService
    {
        public List<(User User, Event Event)> Requests { get; } = new();

        public Task NotifyAsync(User user, Event worldEvent)
        {
            Requests.Add((user, worldEvent));
            return Task.CompletedTask;
        }
    }
}
