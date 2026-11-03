using notif_be.Models;
using notif_be.Repositories;

namespace notif_be.Services;

public class EventProcessor(IUserRepository userRepository, INotifyService notifyService)
{
    public async Task<IReadOnlyList<(int UserId, bool Success)>> ProcessAsync(Event worldEvent)
    {
        ArgumentNullException.ThrowIfNull(worldEvent);

        var results = new List<(int UserId, bool Success)>();

        foreach (var user in userRepository.GetAll())
        {
            if (user.AlertRule?.Matches(worldEvent.EventType) == true)
            {
                try
                {
                    await notifyService.NotifyAsync(user, worldEvent);
                    results.Add((user.Id, true));
                }
                catch
                {
                    results.Add((user.Id, false));
                }
            }
        }

        return results;
    }
}
