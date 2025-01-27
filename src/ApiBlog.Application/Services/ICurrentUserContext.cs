namespace ApiBlog.Application.Services;

public interface ICurrentUserContext
{
    string? Username { get; }
}