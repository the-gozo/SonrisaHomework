namespace notif_be.Models;

public class User : IModel 
{
    public int Id { get; set; }
    public string Email { get; }
    public AlertRule? AlertRule { get; }

    public User(int id, string email, AlertRule? alertRule = null)
    {
        Id = id;
        Email = email;
        AlertRule = alertRule;
    }
}
