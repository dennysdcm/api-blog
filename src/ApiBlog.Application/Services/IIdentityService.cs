using System.Threading.Tasks;
using ApiBlog.Application.Dtos.Identity;
using ApiBlog.Domain.Entity;
using ApiBlog.Domain.Repositories;

namespace ApiBlog.Application.Services;

public interface IIdentityService
{
    Task<IdentityResponse> RegisterAsync(IdentityRequest request);
    Task<IdentityResponse> AuthenticateAsync(IdentityRequest request);
}

public class IdentityService : IIdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IHashProvider _hashProvider;

    public IdentityService(IUserRepository userRepository, ITokenGenerator tokenGenerator, IHashProvider hashProvider)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _hashProvider = hashProvider;
    }

    public async Task<IdentityResponse> RegisterAsync(IdentityRequest request)
    {
        var user = await _userRepository.GetByUserNameAsync(request.Username);
        if (user != null) return null;
        
        var newUser = new User(request.Username, _hashProvider.Hash(request.Password));
        await _userRepository.AddAsync(newUser);
        
        return new IdentityResponse(newUser.Username, _tokenGenerator.GenerateToken(newUser.Username));
    }

    public async Task<IdentityResponse> AuthenticateAsync(IdentityRequest request)
    {
        var user = await _userRepository.GetByUserNameAsync(request.Username);
        
        if (user == null) return null;
        if (!_hashProvider.Verify(request.Password, user.Password)) return null;
        
        return new IdentityResponse(user.Username, _tokenGenerator.GenerateToken(user.Username));
    }
}