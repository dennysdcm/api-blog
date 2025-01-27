using ApiBlog.Application.Dtos.Identity;
using ApiBlog.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiBlog.WebApi.Controllers;

[Route("api/identity")]
public class IdentityController : BaseController
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    [SwaggerOperation(Description = "Register a user in the Identity provider.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdentityResponse>> Register([FromBody] IdentityRequest request)
    {
        var response = await _identityService.RegisterAsync(request);
        
        if (response is null)
        {
            return BadRequest(new { Error = "Username already exists." });
        }
        
        return Ok(response);
    }
    
    [HttpPost("authenticate")]
    [SwaggerOperation(Description = "Authenticates a user and generates a token to be used in requests.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdentityResponse>> Authenticate([FromBody] IdentityRequest request)
    {
        var response = await _identityService.AuthenticateAsync(request);
        
        if (response is null)
        {
            return BadRequest(new { Error = "Username or Password is incorrect." });
        }

        return Ok(response);
    }
}