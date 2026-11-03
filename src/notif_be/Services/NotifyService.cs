using notif_be.Models;

namespace notif_be.Services;

public class NotifyService : INotifyService
{
    public NotifyService(IEmailService emailService)
    {
    }

    public Task NotifyAsync(User user, Event worldEvent)
    {
        return Task.CompletedTask;
    }
}
