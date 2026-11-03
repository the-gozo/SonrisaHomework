using notif_be.Models;
using notif_be.Repositories;

namespace notif_be.Services;

public class EventProcessor(IUserRepository userRepository, INotifyService notifyService)
{
    public async Task ProcessAsync(Event worldEvent)
    {
        foreach (var user in userRepository.GetAll())
        {
            if (user.AlertRule?.Matches(worldEvent.EventType) == true)
            {
                await notifyService.NotifyAsync(user, worldEvent);
            }
        }
    }
}
