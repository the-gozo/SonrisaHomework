using notif_be.Models;

namespace notif_be.Services;

public class NotifyService(IEmailService emailService) : INotifyService
{
    public Task NotifyAsync(User user, Event worldEvent)
    {
        return emailService.SendAsync(user.Email, worldEvent.Title, worldEvent.Body);
    }
}
