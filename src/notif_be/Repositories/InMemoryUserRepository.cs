using notif_be.Models;

namespace notif_be.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly User[] users;

    public InMemoryUserRepository(IEnumerable<User> users)
    {
        this.users = users.ToArray();
    }

    public IEnumerable<User> GetAll() => users;
}
