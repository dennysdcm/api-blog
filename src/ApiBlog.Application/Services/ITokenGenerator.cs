using ApiBlog.Domain.Entity;

namespace ApiBlog.Application.Services;

public interface ITokenGenerator
{
    string GenerateToken(string username);
}