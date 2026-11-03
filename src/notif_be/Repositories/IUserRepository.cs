using notif_be.Models;

namespace notif_be.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAll();
}
