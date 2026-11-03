using notif_be.Models;

namespace notif_be.Services;

public interface INotifyService
{
    Task NotifyAsync(User user, Event worldEvent);
}
