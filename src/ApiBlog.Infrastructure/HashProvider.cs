using ApiBlog.Application.Services;

namespace ApiBlog.Infrastructure;

public class HashProvider : IHashProvider
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string passwordHas)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHas);
    }
}