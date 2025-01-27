namespace ApiBlog.Application.Services;

public interface IHashProvider
{
    string Hash(string password);
    bool Verify(string password, string passwordHas);
}