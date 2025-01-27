using ApiBlog.Application.Services;
using ApiBlog.Domain.Entity;
using ApiBlog.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ApiBlog.UnitTests.Application.Services;

public class IdentityServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IHashProvider _hashProvider;
    
    private readonly IIdentityService _sut;

    public IdentityServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _tokenGenerator = Substitute.For<ITokenGenerator>();
        _hashProvider = Substitute.For<IHashProvider>();

        _sut = new IdentityService(_userRepository, _tokenGenerator, _hashProvider);
    }
    
    [Fact]
    public async Task RegisterAsync__Should_Create_A_New_User()
    {
        _userRepository.GetByUserNameAsync(Arg.Any<string>()).Returns(default(User));
        
        await _sut.RegisterAsync(new ("user", "pwd"));

        await _userRepository.Received(1).AddAsync(Arg.Any<User>());
    }
    
    [Fact]
    public async Task RegisterAsync__Should_Not_Create_A_New_User_When_Already_Exists()
    {
        _userRepository.GetByUserNameAsync(Arg.Any<string>()).Returns(new User("user", "other"));
        
        await _sut.RegisterAsync(new ("user", "pwd"));

        await _userRepository.Received(0).AddAsync(Arg.Any<User>());
    }
    
    [Fact]
    public async Task AuthenticateAsync__Should_Return_A_Token_When_Valid_Credentials()
    {
        _userRepository.GetByUserNameAsync(Arg.Any<string>()).Returns(new User("user", "pwd"));
        _hashProvider.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        _tokenGenerator.GenerateToken(Arg.Any<string>()).Returns("token");
        
        var response = await _sut.AuthenticateAsync(new ("user", "pwd"));

        response.Should().NotBeNull();
        response.Token.Should().Be("token");
        response.Username.Should().Be("user");
    }
    
    [Fact]
    public async Task AuthenticateAsync__Should_Not_Return_A_Token_When_Invalid_Credentials()
    {
        _userRepository.GetByUserNameAsync(Arg.Any<string>()).Returns(new User("user", "pwd"));
        _hashProvider.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
        _tokenGenerator.GenerateToken(Arg.Any<string>()).Returns("token");
        
        var response = await _sut.AuthenticateAsync(new ("user", "pwd"));

        response.Should().BeNull();
    }
}