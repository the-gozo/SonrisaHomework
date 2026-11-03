using notif_be.Models;

namespace notif_be.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    public InMemoryUserRepository(IEnumerable<User> users)
    {
    }

    public IEnumerable<User> GetAll() => throw new NotImplementedException();
}
