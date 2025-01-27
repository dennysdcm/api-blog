using ApiBlog.Application.Services;

namespace ApiBlog.WebApi.Authentication;

public class HttpCurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpCurrentUserContext(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? Username => _contextAccessor?.HttpContext?.User?.Identity?.Name;
}