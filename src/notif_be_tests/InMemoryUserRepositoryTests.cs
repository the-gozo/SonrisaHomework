using notif_be.Models;
using notif_be.Repositories;

namespace notif_be_tests;

public class InMemoryUserRepositoryTests
{
    [Fact]
    public void Retrieval_returns_no_users_when_initialized_with_an_empty_collection()
    {
        IUserRepository repository = new InMemoryUserRepository(Array.Empty<User>());

        var retrievedUsers = repository.GetAll();

        Assert.Empty(retrievedUsers);
    }

    [Fact]
    public void Retrieval_returns_all_supplied_users_with_and_without_alert_rules()
    {
        var rule = new AlertRule(17, new[] { "breaking-news" }, true, false);
        var users = new[]
        {
            new User(42, "alice@example.com", rule),
            new User(43, "bob@example.com")
        };
        IUserRepository repository = new InMemoryUserRepository(users);

        var retrievedUsers = repository.GetAll();

        Assert.Equal(
            new (int Id, string Email, int? AlertRuleId)[]
            {
                (42, "alice@example.com", 17),
                (43, "bob@example.com", null)
            },
            retrievedUsers.OrderBy(user => user.Id)
                .Select(user => (user.Id, user.Email, user.AlertRule?.Id)));
    }
}
