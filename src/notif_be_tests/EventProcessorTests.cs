using notif_be.Models;
using notif_be.Repositories;
using notif_be.Services;

namespace notif_be_tests;

public class EventProcessorTests
{
    [Fact]
    public async Task Processing_records_a_failed_request_without_retrying_and_continues_to_later_users()
    {
        var rule = new AlertRule(17, new[] { "breaking-news" }, true, false);
        var repository = new InMemoryUserRepository(new[]
        {
            new User(42, "alice@example.com", rule),
            new User(43, "bob@example.com", rule)
        });
        var notifyService = new FailingNotifyService(failingUserId: 42);
        var processor = new EventProcessor(repository, notifyService);
        var worldEvent = new Event(7, "breaking-news", "Important news", "Full report");

        var results = await processor.ProcessAsync(worldEvent);

        Assert.Equal(new[] { (42, false), (43, true) }, results);
        Assert.Equal(new[] { 42, 43 }, notifyService.RequestedUserIds);
    }

    [Fact]
    public async Task Processing_the_same_event_twice_requests_notifications_again_for_matching_users()
    {
        var rule = new AlertRule(17, new[] { "breaking-news" }, true, false);
        var repository = new InMemoryUserRepository(new[]
        {
            new User(42, "alice@example.com", rule),
            new User(43, "bob@example.com", rule)
        });
        var notifyService = new RecordingNotifyService();
        var processor = new EventProcessor(repository, notifyService);
        var worldEvent = new Event(7, "breaking-news", "Important news", "Full report");

        var firstResults = await processor.ProcessAsync(worldEvent);

        Assert.Equal(new[] { 42, 43 }, notifyService.Requests.Select(request => request.User.Id));

        var secondResults = await processor.ProcessAsync(worldEvent);

        Assert.Equal(new[] { 42, 43, 42, 43 },
            notifyService.Requests.Select(request => request.User.Id));
        Assert.All(notifyService.Requests, request => Assert.Same(worldEvent, request.Event));
        Assert.Equal(new[] { (42, true), (43, true) }, firstResults);
        Assert.Equal(new[] { (42, true), (43, true) }, secondResults);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Processing_returns_no_results_or_notification_requests_when_no_users_match(
        bool includeNonmatchingUsers)
    {
        var nonmatchingRule = new AlertRule(17, new[] { "market-movement" }, true, false);
        var users = includeNonmatchingUsers
            ? new[]
            {
                new User(42, "alice@example.com", nonmatchingRule),
                new User(43, "bob@example.com")
            }
            : Array.Empty<User>();
        var repository = new InMemoryUserRepository(users);
        var notifyService = new RecordingNotifyService();
        var processor = new EventProcessor(repository, notifyService);
        var worldEvent = new Event(7, "breaking-news", "Important news", "Full report");

        var results = await processor.ProcessAsync(worldEvent);

        Assert.Empty(results);
        Assert.Empty(notifyService.Requests);
    }

    [Fact]
    public async Task Processing_returns_a_successful_result_when_a_users_notification_request_completes()
    {
        var rule = new AlertRule(17, new[] { "breaking-news" }, true, false);
        var repository = new InMemoryUserRepository(new[]
        {
            new User(42, "alice@example.com", rule)
        });
        var notifyService = new RecordingNotifyService();
        var processor = new EventProcessor(repository, notifyService);
        var worldEvent = new Event(7, "breaking-news", "Important news", "Full report");

        var results = await processor.ProcessAsync(worldEvent);

        var result = Assert.Single(results);
        Assert.Equal(42, result.UserId);
        Assert.True(result.Success);
    }

    [Theory]
    [InlineData(42, 1)]
    [InlineData(43, 2)]
    public async Task Processing_waits_for_each_notification_before_proceeding_or_completing(
        int blockedUserId, int expectedRequestsBeforeRelease)
    {
        var rule = new AlertRule(17, new[] { "breaking-news" }, true, false);
        var repository = new InMemoryUserRepository(new[]
        {
            new User(42, "alice@example.com", rule),
            new User(43, "bob@example.com", rule)
        });
        var notifyService = new BlockingNotifyService(blockedUserId);
        var processor = new EventProcessor(repository, notifyService);
        var worldEvent = new Event(7, "breaking-news", "Important news", "Full report");

        var processing = processor.ProcessAsync(worldEvent);

        try
        {
            await notifyService.BlockedRequestStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));

            Assert.Equal(expectedRequestsBeforeRelease, notifyService.RequestedUserIds.Count);
            Assert.False(processing.IsCompleted);
        }
        finally
        {
            notifyService.ReleaseBlockedRequest();
            await processing.WaitAsync(TimeSpan.FromSeconds(5));
        }

        Assert.Equal(new[] { 42, 43 }, notifyService.RequestedUserIds.ToArray());
    }

    [Fact]
    public async Task Processing_preserves_the_event_title_and_body_verbatim()
    {
        var rule = new AlertRule(17, new[] { "breaking-news" }, true, false);
        var repository = new InMemoryUserRepository(new[]
        {
            new User(42, "alice@example.com", rule)
        });
        var notifyService = new RecordingNotifyService();
        var processor = new EventProcessor(repository, notifyService);
        var worldEvent = new Event(7, "breaking-news",
            "  Important news!  ", "\tFirst line.\r\nSecond line.\n  ");

        await processor.ProcessAsync(worldEvent);

        var request = Assert.Single(notifyService.Requests);
        Assert.Equal("  Important news!  ", request.Event.Title);
        Assert.Equal("\tFirst line.\r\nSecond line.\n  ", request.Event.Body);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public async Task Processing_requests_one_notification_with_the_users_selected_channels(
        bool emailEnabled, bool slackEnabled)
    {
        var rule = new AlertRule(17, new[] { "breaking-news" }, emailEnabled, slackEnabled);
        var repository = new InMemoryUserRepository(new[]
        {
            new User(42, "alice@example.com", rule)
        });
        var notifyService = new RecordingNotifyService();
        var processor = new EventProcessor(repository, notifyService);
        var worldEvent = new Event(7, "breaking-news", "Important news", "Full report");

        await processor.ProcessAsync(worldEvent);

        var request = Assert.Single(notifyService.Requests);
        Assert.Equal(42, request.User.Id);
        Assert.Same(worldEvent, request.Event);
        var selectedRule = Assert.IsType<AlertRule>(request.User.AlertRule);
        Assert.Equal((emailEnabled, slackEnabled),
            (selectedRule.EmailEnabled, selectedRule.SlackEnabled));
    }

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

        var results = await processor.ProcessAsync(worldEvent);

        var request = Assert.Single(notifyService.Requests);
        Assert.Equal(42, request.User.Id);
        Assert.Same(worldEvent, request.Event);
        var result = Assert.Single(results);
        Assert.Equal(42, result.UserId);
    }

    private sealed class FailingNotifyService(int failingUserId) : INotifyService
    {
        public List<int> RequestedUserIds { get; } = new();

        public Task NotifyAsync(User user, Event worldEvent)
        {
            RequestedUserIds.Add(user.Id);
            return user.Id == failingUserId
                ? Task.FromException(new InvalidOperationException("Notification request failed."))
                : Task.CompletedTask;
        }
    }

    private sealed class BlockingNotifyService(int blockedUserId) : INotifyService
    {
        private readonly TaskCompletionSource completion =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource BlockedRequestStarted { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        public System.Collections.Concurrent.ConcurrentQueue<int> RequestedUserIds { get; } = new();

        public Task NotifyAsync(User user, Event worldEvent)
        {
            RequestedUserIds.Enqueue(user.Id);
            if (user.Id == blockedUserId)
            {
                BlockedRequestStarted.SetResult();
                return completion.Task;
            }

            return Task.CompletedTask;
        }

        public void ReleaseBlockedRequest() => completion.TrySetResult();
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
