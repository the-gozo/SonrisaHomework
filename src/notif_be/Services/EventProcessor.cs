using notif_be.Models;
using notif_be.Repositories;

namespace notif_be.Services;

public class EventProcessor
{
    public EventProcessor(IUserRepository userRepository, INotifyService notifyService)
    {
    }

    public Task ProcessAsync(Event worldEvent) => throw new NotImplementedException();
}
