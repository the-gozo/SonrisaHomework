namespace notif_be;

public class User
{
    public int Id { get; }
    public string Email { get; }
    public AlertRule? AlertRule { get; }

    public User(int id, string email, AlertRule? alertRule = null)
    {
        Id = id;
        Email = email;
        AlertRule = alertRule;
    }
}
