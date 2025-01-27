namespace ApiBlog.Domain.Entity;

public class User : AggregateRoot<string>
{
    public string? Username { get; private set; }
    public string? Password { get; private set; }
    
    private User()
    {
    }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}